using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }

        public IConfiguration Configuration { get; }
        private readonly SwaggerOptions _swaggerOptions = new SwaggerOptions();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddYetAnotherTodoAppDbContext(Configuration.GetConnectionString("DefaultConnection"));
            services.AddControllers();
            services.AddSwaggerConfiguration(_swaggerOptions);
            services.RegisterRepositoriesModule();
            services.RegisterCommandsModule();
            services.RegisterHelpersModule();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.AddSwaggerMiddleware(_swaggerOptions);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
