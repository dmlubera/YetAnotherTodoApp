using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Handlers;
using YetAnotherTodoApp.Application.Queries.Models;

namespace YetAnotherTodoApp.Application.DI
{
    public static class QueriesModuleInstaller
    {
        public static void RegisterQueriesModule(this IServiceCollection services)
        {
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<IQueryHandler<GetTodosQuery, IEnumerable<TodoDto>>, GetTodosQueryHandler>();
            services.AddScoped<IQueryHandler<GetTodoListsQuery, IEnumerable<TodoListDto>>, GetTodoListsQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserInfoQuery, UserInfoDto>, GetUserInforQueryHandler>();
        }
    }
}
