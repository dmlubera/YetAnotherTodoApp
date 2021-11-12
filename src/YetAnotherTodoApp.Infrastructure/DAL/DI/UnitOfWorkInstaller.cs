using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Infrastructure.DAL.UnitOfWork;

namespace YetAnotherTodoApp.Infrastructure.DAL.DI
{
    public static class UnitOfWorkInstaller
    {
        public static void AddUnitOfWork(this IServiceCollection services)
            => services.AddScoped<IUnitOfWork, UnitOfWork<YetAnotherTodoAppDbContext>>();
    }
}