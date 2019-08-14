using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace MediatR.AspNetCore.Mvc
{
    public class ActionModelConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (action.Controller.ControllerType.IsGenericType)
            {
                var genericType = action.Controller.ControllerType.GenericTypeArguments[0];
                var customNameAttribute = genericType.GetCustomAttribute<MediatRControllerAttribute>();
                if (customNameAttribute != null)
                {
                    action.ActionName = genericType.Name;
                    action.Selectors.Clear();
                    var selectorModel = new SelectorModel();
                    var route = GetRouteTemplateProvider(customNameAttribute);
                    selectorModel.AttributeRouteModel = new AttributeRouteModel(route);
                    selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(route.HttpMethods));
                    action.Selectors.Add(selectorModel);
                }
            }
        }

        protected virtual HttpMethodAttribute GetRouteTemplateProvider(MediatRControllerAttribute attribute)
        {
            HttpMethodAttribute httpMethodAttribute = null;
            switch (attribute.Method)
            {
                case HttpMethodEnum.Delete:
                    httpMethodAttribute = new Microsoft.AspNetCore.Mvc.HttpDeleteAttribute(attribute.Template);
                    break;
                case HttpMethodEnum.Get:
                    httpMethodAttribute = new Microsoft.AspNetCore.Mvc.HttpGetAttribute(attribute.Template);
                    break;
                //case HttpMethodEnum.Head:
                //    httpMethodAttribute = new Microsoft.AspNetCore.Mvc.HttpHeadAttribute(attribute.Template);
                //    break;
                //case HttpMethodEnum.Options:
                //    httpMethodAttribute = new Microsoft.AspNetCore.Mvc.HttpOptionsAttribute(attribute.Template);
                //    break;
                //case HttpMethodEnum.Patch:
                //    httpMethodAttribute = new Microsoft.AspNetCore.Mvc.HttpPatchAttribute(attribute.Template);
                //    break;
                case HttpMethodEnum.Post:
                    httpMethodAttribute = new Microsoft.AspNetCore.Mvc.HttpPostAttribute();
                    break;
                case HttpMethodEnum.Put:
                    httpMethodAttribute = new Microsoft.AspNetCore.Mvc.HttpPutAttribute();
                    break;
                default:
                    httpMethodAttribute = new Microsoft.AspNetCore.Mvc.HttpGetAttribute(attribute.Template);
                    break;
            }
            return httpMethodAttribute;
        }
    }
}
