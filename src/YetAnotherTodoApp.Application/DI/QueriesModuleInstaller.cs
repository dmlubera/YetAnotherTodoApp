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
            services.AddTransient<IQueryHandler<GetTodosQuery, IEnumerable<TodoDto>>, GetTodosQueryHandler>();
            services.AddTransient<IQueryHandler<GetTodoListsQuery, IEnumerable<TodoListDto>>, GetTodoListsQueryHandler>();
            services.AddTransient<IQueryHandler<GetTodoListQuery, TodoListDto>, GetTodoListQueryHandler>();
            services.AddTransient<IQueryHandler<GetUserInfoQuery, UserInfoDto>, GetUserInforQueryHandler>();
            services.AddTransient<IQueryHandler<GetTodoQuery, TodoDto>, GetTodoQueryHandler>();
        }
    }
}