using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<TodoTask> GetForUserAsync(Guid taskId, Guid userId);
        Task UpdateAsync(TodoTask task);
        Task SaveChangesAsync();
    }
}