using Microsoft.EntityFrameworkCore;
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
           => await _dbContext.TodoLists.Where(x => x.User.Id == userId).ToListAsync();

        public async Task<TodoList> GetByBelongTodo(Guid userId, Guid todoId)
            => await _dbContext.TodoLists
                .Include(x => x.Todos)
                .FirstOrDefaultAsync(x => x.User.Id == userId && x.Todos.Any(x => x.Id == todoId));

        public async Task<TodoList> GetForUserAsync(Guid userId, Guid todoListId)
            => await _dbContext.TodoLists.FirstOrDefaultAsync(x => x.User.Id == userId && x.Id == todoListId);

        public async Task<bool> CheckIfUserHasGotTodoListWithGivenTitle(Guid userId, string title)
            => await _dbContext.TodoLists.AnyAsync(x => x.User.Id == userId && x.Title.Value == title);

        public async Task UpdateAsync(TodoList todoList)
        {
            _dbContext.Update(todoList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();
    }
}