using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MediatR.AspNetCore.Mvc
{
    internal class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private readonly IPolicyEvaluator _policyEvaluator;

        private AuthorizationPolicy authorizePolicy;

        public AuthorizationFilter(
              IAuthorizationPolicyProvider policyProvider,
              IPolicyEvaluator policyEvaluator
              )
        {
            _policyProvider = policyProvider;
            _policyEvaluator = policyEvaluator;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                if (controllerActionDescriptor.ControllerTypeInfo.IsGenericType)
                {
                    var genericType = controllerActionDescriptor.ControllerTypeInfo.GenericTypeArguments[0];
                    var customNameAttribute = genericType.GetCustomAttribute<MediatRControllerAttribute>();
                    if (customNameAttribute != null)
                    {
                        var requestPolicies = genericType.GetCustomAttributes<AuthorizeAttribute>(inherit: true);

                        authorizePolicy = await AuthorizationPolicy.CombineAsync(_policyProvider, requestPolicies);
                        if (requestPolicies.Count() == 0) return;
                        AuthenticateResult authenticateResult =
                           await _policyEvaluator.AuthenticateAsync(authorizePolicy, context.HttpContext);

                        PolicyAuthorizationResult authorizeResult =
                            await _policyEvaluator.AuthorizeAsync(authorizePolicy, authenticateResult, context.HttpContext, null);

                        if (authorizeResult.Challenged)
                        {
                            await ChallengeAsync(context);
                        }
                        else if (authorizeResult.Forbidden)
                        {
                            await ForbidAsync(context);
                        }
                    }
                }
            }
        }

        private async Task ChallengeAsync(AuthorizationFilterContext context)
        {
            if (authorizePolicy.AuthenticationSchemes.Count > 0)
            {
                foreach (string authenticationScheme in authorizePolicy.AuthenticationSchemes)
                {
                    await context.HttpContext.ChallengeAsync(authenticationScheme);
                }
            }
            await context.HttpContext.ChallengeAsync();
        }

        private async Task ForbidAsync(AuthorizationFilterContext context)
        {
            if (authorizePolicy.AuthenticationSchemes.Count > 0)
            {
                foreach (string authenticationScheme in authorizePolicy.AuthenticationSchemes)
                {
                    await context.HttpContext.ForbidAsync(authenticationScheme);
                }
            }
            await context.HttpContext.ForbidAsync();
        }
    }
}
