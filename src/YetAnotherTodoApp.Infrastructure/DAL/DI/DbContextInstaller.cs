using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace YetAnotherTodoApp.Infrastructure.DAL.DI
{
    public static class DbContextInstaller
    {
        public static void AddYetAnotherTodoAppDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<YetAnotherTodoAppDbContext>(opts =>
            {
                opts.UseSqlServer(connectionString);
                opts.UseLazyLoadingProxies();
            });
        }
    }
}