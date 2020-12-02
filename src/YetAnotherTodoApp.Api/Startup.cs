using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Api.Configurations;
using YetAnotherTodoApp.Api.Middlewares;
using YetAnotherTodoApp.Application.DI;
using YetAnotherTodoApp.Infrastructure.Auth.DI;
using YetAnotherTodoApp.Infrastructure.DAL.DI;

namespace YetAnotherTodoApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddYetAnotherTodoAppDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.AddControllers();
            services.AddOptions();
            services.AddSwaggerConfiguration(Configuration);
            services.AddAuthenticationConfiguration(Configuration);
            services.AddMemoryCache();
            services.RegisterRepositoriesModule();
            services.RegisterCommandsModule();
            services.RegisterQueriesModule();
            services.RegisterHelpersModule();
            services.RegisterAuthModule();
            services.RegisterAutoMapperModule();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.AddSwaggerMiddleware(Configuration);

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
