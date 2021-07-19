using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Infrastructure.DAL.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly YetAnotherTodoAppDbContext _dbContext;

        public TodoRepository(YetAnotherTodoAppDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<IList<Todo>> GetAllForUserAsync(Guid userId)
            => await _dbContext.Set<Todo>()
                .Include(x => x.TodoList)
                .Include(x => x.Steps)
                .Where(x => x.TodoList.User.Id == userId).ToListAsync();

        public async Task<Todo> GetForUserAsync(Guid todoId, Guid userId)
            => await _dbContext.Set<Todo>()
                .Include(x => x.TodoList)
                .Include(x => x.Steps)
                .FirstOrDefaultAsync(x => x.Id == todoId && x.TodoList.User.Id == userId);

        public async Task SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();
    }
}