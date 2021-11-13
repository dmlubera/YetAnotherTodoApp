using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Infrastructure.DAL.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly YetAnotherTodoAppDbContext _dbContext;

        public TodoTaskRepository(YetAnotherTodoAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TodoTask> GetForUserAsync(Guid taskId, Guid userId)
            => await _dbContext.TodoTasks.FirstOrDefaultAsync(x => x.Id == taskId && x.Todo.TodoList.User.Id == userId);

        public async Task UpdateAsync(TodoTask task)
        {
            _dbContext.Update(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();
    }
}