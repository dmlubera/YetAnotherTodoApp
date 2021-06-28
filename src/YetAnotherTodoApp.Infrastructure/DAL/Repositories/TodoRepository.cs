using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<Todo> GetTodoAsync(Guid id)
            => await _dbContext.Set<Todo>().Include(x => x.TodoList).FirstOrDefaultAsync(x => x.Id == id);
    }
}