using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Cache;

namespace YetAnotherTodoApp.Infrastructure.Cache.DI
{
    internal static class CacheModuleInstaller
    {
        internal static void RegisterCacheModule(this IServiceCollection services)
            => services.AddScoped<ICache, MemoryCache>();
    }
}