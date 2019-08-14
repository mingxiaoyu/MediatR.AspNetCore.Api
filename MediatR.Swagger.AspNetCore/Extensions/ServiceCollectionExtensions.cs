using QiongHua.MediatR.Swagger.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        [Obsolete]
        public static IServiceCollection AddMediatRSwaggerGen(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<MediatRControllerOperationFilter>();
            });
            services.AddSwaggerGen(setupAction);
            return services;
        }

    }
}
