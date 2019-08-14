using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace MediatR.AspNetCore.Mvc
{
    public class ControllerRouteConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                var genericType = controller.ControllerType.GenericTypeArguments[0];
                var customNameAttribute = genericType.GetCustomAttribute<MediatRControllerAttribute>();
                if (customNameAttribute != null)
                {
                    if (customNameAttribute.Route != null)
                    {
                        controller.Selectors.Clear();
                        controller.Selectors.Add(new SelectorModel
                        {
                            AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(customNameAttribute.Route)),
                        });

                    }
                    controller.ControllerName = $"{customNameAttribute.Controller}";
                }
            }
        }
    }
}
