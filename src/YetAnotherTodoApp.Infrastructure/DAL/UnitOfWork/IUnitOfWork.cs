using System;
using System.Threading.Tasks;

namespace YetAnotherTodoApp.Infrastructure.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task ExecuteAsync(Func<Task> action);
    }
}