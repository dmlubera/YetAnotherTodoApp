using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Cache;

namespace YetAnotherTodoApp.Infrastructure.Cache.DI
{
    public static class CacheModuleInstaller
    {
        public static void RegisterCacheModule(this IServiceCollection services)
            => services.AddScoped<ICache, MemoryCache>();
    }
}
