using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries;
using YetAnotherTodoApp.Application.Queries.Handlers.TodoLists;
using YetAnotherTodoApp.Application.Queries.Handlers.Todos;
using YetAnotherTodoApp.Application.Queries.Handlers.Users;
using YetAnotherTodoApp.Application.Queries.Models.TodoLists;
using YetAnotherTodoApp.Application.Queries.Models.Todos;
using YetAnotherTodoApp.Application.Queries.Models.Users;

namespace YetAnotherTodoApp.Application.DI
{
    internal static class QueriesModuleInstaller
    {
        internal static void RegisterQueriesModule(this IServiceCollection services)
        {
            services.AddScoped<IQueryHandler<GetTodosQuery, IEnumerable<TodoDto>>, GetTodosQueryHandler>();
            services.AddScoped<IQueryHandler<GetTodoListsQuery, IEnumerable<TodoListDto>>, GetTodoListsQueryHandler>();
            services.AddScoped<IQueryHandler<GetTodoListQuery, TodoListDto>, GetTodoListQueryHandler>();
            services.AddScoped<IQueryHandler<GetUserInfoQuery, UserInfoDto>, GetUserInforQueryHandler>();
            services.AddScoped<IQueryHandler<GetTodoQuery, TodoDto>, GetTodoQueryHandler>();
        }
    }
}