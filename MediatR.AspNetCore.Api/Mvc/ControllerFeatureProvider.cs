using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.AspNetCore.Mvc
{
    public class ControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private const string ControllerTypeNameSuffix = "Controller";
        private Assembly[] _assemblies;
        private static Type RequestType = typeof(IRequest<>);

        public ControllerFeatureProvider(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new List<Assembly>() { typeof(ControllerFeatureProvider).Assembly }.ToArray();
            }
            _assemblies = assemblies;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var candidates = _assemblies.SelectMany(x => x.GetExportedTypes().Where(y => y.GetCustomAttributes<MediatRControllerAttribute>().Any()));

            foreach (var type in candidates)
            {
                if (IsMediatR(type) && !feature.Controllers.Any(t => t.Name == type.Name))
                {
                    var request = type.FindGenericType(RequestType);

                    var arguments = request.GetGenericArguments()[0];

                    var mediatRController = typeof(MediatRController<,>).MakeGenericType(type, arguments).GetTypeInfo();
                    feature.Controllers.Add(mediatRController);

                }
            }
        }

        protected virtual bool IsMediatR(Type typeInfo)
        {
            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            // We only consider public top-level classes as controllers. IsPublic returns false for nested
            // classes, regardless of visibility modifiers
            if (!typeInfo.IsPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            return typeInfo.IsKindOfGeneric(RequestType);
        }
    }
}

