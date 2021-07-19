using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Repositories
{
    public interface IStepRepository
    {
        Task<Step> GetForUserAsync(Guid stepId, Guid userId);
        Task SaveChangesAsync();
    }
}