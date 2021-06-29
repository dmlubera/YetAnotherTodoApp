using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Repositories
{
    public interface ITodoRepository
    {
        Task<Todo> GetTodoAsync(Guid id);
    }
}
