using FluentValidation.AspNetCore;
using MediatR;
using MediatR.AspNetCore.Mvc;
using MediatR.Extensions.FluentValidation;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [Obsolete]
        public static void ConfigureMediatRApi(this IServiceCollection services, params Assembly[] assemblies)
        {
            services
                .AddMvcCore(options =>
                {
                    options.Conventions.Add(new ControllerRouteConvention());
                    options.Conventions.Add(new ActionModelConvention());
                    options.Conventions.Add(new ParameterModelConvention());
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new ControllerFeatureProvider(assemblies));
                })
                .AddFluentValidation(fv =>
                 {
                     fv.RegisterValidatorsFromAssemblies(assemblies);
                     fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                 });

            services.AddMediatR(assemblies);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        }
    }
}
