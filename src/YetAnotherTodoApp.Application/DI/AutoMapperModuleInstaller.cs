using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Mappers.Profiles;

namespace YetAnotherTodoApp.Application.DI
{
    public static class AutoMapperModuleInstaller
    {
        public static void RegisterAutoMapperModule(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TodoProfile());
                cfg.AddProfile(new TodoListProfile());
            });

            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
