using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Domain.Repositories;
using YetAnotherTodoApp.Infrastructure.DAL.Repositories;

namespace YetAnotherTodoApp.Infrastructure.DAL.DI
{
    public static class RepositoriesModuleInstaller
    {
        public static void RegisterRepositoriesModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}