using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MediatR.AspNetCore.Mvc;
using MediatR.Extensions.FluentValidation;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcCoreMvcBuilderExtensions
    {
        public static IMvcBuilder AddMediatRApi(this IMvcBuilder builder, params Assembly[] assemblies)
        {
            builder.Services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new ControllerRouteConvention());
                options.Conventions.Add(new ActionModelConvention());
                options.Conventions.Add(new ParameterModelConvention());
            });

            builder.ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new ControllerFeatureProvider(assemblies));
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblies(assemblies);
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            });

            builder.Services.AddMediatR(assemblies);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            return builder;
        }
    }
}
