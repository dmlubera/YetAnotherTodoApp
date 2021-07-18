using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using YetAnotherTodoApp.Application.Mappers.Profiles;

namespace YetAnotherTodoApp.Application.DI
{
    public static class AutoMapperModuleInstaller
    {
        internal static void RegisterAutoMapperModule(this IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TodoProfile());
                cfg.AddProfile(new TodoListProfile());
                cfg.AddProfile(new UserInfoProfile());
                cfg.AddProfile(new StepProfile());
                cfg.AddProfile(new StepRequestDtoProfile());
            });

            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}