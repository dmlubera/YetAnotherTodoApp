using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Repositories
{
    public interface ITodoListRepository
    {
        Task<IEnumerable<TodoList>> GetAllForUserAsync(Guid userId);
        Task<TodoList> GetByBelongTodo(Guid userId, Guid todoId);
        Task<TodoList> GetForUserAsync(Guid userId, Guid todoListId);
        Task<bool> CheckIfUserHasGotTodoListWithGivenTitle(Guid userId, string title);
        Task UpdateAsync(TodoList todoList);
    }
}
