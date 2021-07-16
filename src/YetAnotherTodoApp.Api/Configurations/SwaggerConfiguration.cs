using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Settings;

namespace YetAnotherTodoApp.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = GetSwaggerSettings(configuration);

            services.Configure<SwaggerSettings>(GetSwaggerSection(configuration));

            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = swaggerSettings.Name
                });

                opts.ExampleFilters();

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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        }

        public static void AddSwaggerMiddleware(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerSettings = GetSwaggerSettings(configuration);
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint(swaggerSettings.UIEndpoint, swaggerSettings.Name));
        }

        private static IConfigurationSection GetSwaggerSection(IConfiguration configuration)
            => configuration.GetSection(nameof(SwaggerSettings));
        
        private static SwaggerSettings GetSwaggerSettings(IConfiguration configuration)
            => GetSwaggerSection(configuration).Get<SwaggerSettings>();
    }
}