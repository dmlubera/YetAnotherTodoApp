using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using YetAnotherTodoApp.Api.Settings;

namespace YetAnotherTodoApp.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = GetSwaggerSettings(configuration);

            services.AddOptions<SwaggerSettings>();

            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = swaggerSettings.Name
                });

                opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using Bearer scheme.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                opts.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void AddSwaggerMiddleware(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerSettings = GetSwaggerSettings(configuration);
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint(swaggerSettings.UIEndpoint, swaggerSettings.Name));
        }

        private static SwaggerSettings GetSwaggerSettings(IConfiguration configuration)
        {
            var swaggerSettings = new SwaggerSettings();
            configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerSettings);

            return swaggerSettings;
        }
    }
}