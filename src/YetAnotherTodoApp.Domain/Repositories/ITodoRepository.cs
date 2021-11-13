using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Repositories
{
    public interface ITodoRepository
    {
        Task<IList<Todo>> GetAllForUserAsync(Guid userId);
        Task<Todo> GetForUserAsync(Guid todoId, Guid userId);
        Task UpdateAsync(Todo todo);
    }
}