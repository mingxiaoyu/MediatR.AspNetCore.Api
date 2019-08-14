using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace MediatR.AspNetCore.Mvc
{
    public class ParameterModelConvention : IParameterModelConvention
    {
        public void Apply(ParameterModel parameter)
        {
            if (parameter.Action.Controller.ControllerType.IsGenericType)
            {
                var genericType = parameter.ParameterType;
                var customNameAttribute = genericType.GetCustomAttribute<MediatRControllerAttribute>();
                if (customNameAttribute != null)
                {
                    if (customNameAttribute.Method == HttpMethodEnum.Get || customNameAttribute.Method == HttpMethodEnum.Delete)
                        parameter.BindingInfo = null;
                }
            }
        }
    }
}
