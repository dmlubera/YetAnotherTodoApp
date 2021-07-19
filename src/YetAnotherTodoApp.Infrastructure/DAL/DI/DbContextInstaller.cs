using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YetAnotherTodoApp.Infrastructure.DAL.DI
{
    public static class DbContextInstaller
    {
        public static void AddYetAnotherTodoAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<YetAnotherTodoAppDbContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}