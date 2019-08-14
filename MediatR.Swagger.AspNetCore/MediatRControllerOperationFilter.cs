using MediatR.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace MediatR.Swagger.AspNetCore
{
    public class MediatRControllerOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                if (controllerActionDescriptor.ControllerTypeInfo.IsGenericType)
                {
                    var genericType = controllerActionDescriptor.ControllerTypeInfo.GenericTypeArguments[0];
                    var customNameAttribute = genericType.GetCustomAttribute<MediatRControllerAttribute>();
                    if (customNameAttribute != null)
                    {
                        operation.OperationId = genericType.Name;
                    }
                }
            }
        }
    }
}
