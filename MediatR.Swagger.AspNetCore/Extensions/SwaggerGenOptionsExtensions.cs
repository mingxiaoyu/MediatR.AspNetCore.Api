using QiongHua.MediatR.Swagger.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void MediatRApiOperationFilter(this SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.OperationFilter<MediatRControllerOperationFilter>();
        }
    }
}
