using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Infrastructure.DAL.Repositories
{
    public class TodoListRepository : ITodoListRepository
    {
        private readonly YetAnotherTodoAppDbContext _dbContext;

        public TodoListRepository(YetAnotherTodoAppDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<IEnumerable<TodoList>> GetAllForUserAsync(Guid userId)
           => await Task.FromResult(_dbContext.TodoLists.Where(x => x.User.Id == userId).ToList());
    }
}
