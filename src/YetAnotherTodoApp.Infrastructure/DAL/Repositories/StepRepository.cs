using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Infrastructure.DAL.Repositories
{
    public class StepRepository : IStepRepository
    {
        private readonly YetAnotherTodoAppDbContext _dbContext;

        public StepRepository(YetAnotherTodoAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Step> GetForUserAsync(Guid stepId, Guid userId)
            => await _dbContext.Steps.FirstOrDefaultAsync(x => x.Id == stepId && x.Todo.TodoList.User.Id == userId);

        public async Task SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();
    }
}