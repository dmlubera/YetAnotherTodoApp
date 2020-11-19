using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using YetAnotherTodoApp.Infrastructure.Auth.Settings;

namespace YetAnotherTodoApp.Api.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static void AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

            services.AddOptions<JwtSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(opts =>
              {
                  opts.SaveToken = true;
                  opts.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                      ValidateIssuer = false,
                      ValidateAudience = false,
                      RequireExpirationTime = true,
                      ValidateLifetime = true
                  };
              });
        }
    }
}