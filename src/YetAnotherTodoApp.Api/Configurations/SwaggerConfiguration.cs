using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using YetAnotherTodoApp.Api.Options;

namespace YetAnotherTodoApp.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, SwaggerOptions swaggerOptions)
        {
            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = swaggerOptions.Name
            }));
        }

        public static void AddSwaggerMiddleware(this IApplicationBuilder app, SwaggerOptions swaggerOptions)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Name));
        }
    }
}