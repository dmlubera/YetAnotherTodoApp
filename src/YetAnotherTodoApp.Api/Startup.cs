using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using YetAnotherTodoApp.Api.Configurations;
using YetAnotherTodoApp.Api.Middlewares;
using YetAnotherTodoApp.Api.Options;
using YetAnotherTodoApp.Application.DI;
using YetAnotherTodoApp.Infrastructure.DAL.DI;

namespace YetAnotherTodoApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(_swaggerOptions);
            Configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);
        }

        public IConfiguration Configuration { get; }
        private readonly SwaggerOptions _swaggerOptions = new SwaggerOptions();
        private readonly JwtOptions _jwtOptions = new JwtOptions();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddYetAnotherTodoAppDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.AddControllers();
            services.AddSwaggerConfiguration(_swaggerOptions);
            services.RegisterRepositoriesModule();
            services.RegisterCommandsModule();
            services.RegisterHelpersModule();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opts => 
                    {
                        opts.SaveToken = true;
                        opts.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Secret)),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            RequireExpirationTime = true,
                            ValidateLifetime = true
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.AddSwaggerMiddleware(_swaggerOptions);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
