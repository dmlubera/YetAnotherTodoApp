using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Infrastructure.DAL;

namespace YetAnotherTodoApp.Tests.End2End.Helpers
{
    public static class DbContextHelper
    {
        public async static Task<IList<TEntity>> GetAsync<TEntity>(this YetAnotherTodoAppDbContext dbContext)
            where TEntity : BaseEntity
            => await dbContext.Set<TEntity>().ToListAsync();

        public async static Task<TEntity> GetAsync<TEntity>(this YetAnotherTodoAppDbContext dbContext, Guid id)
            where TEntity : BaseEntity
            => await dbContext.Set<TEntity>().FindAsync(id);

        public async static Task<Todo> GetTodoWithReferencedTodoListAsync(this YetAnotherTodoAppDbContext dbContext, Guid id)
            => await dbContext
                        .Set<Todo>()
                            .Include(x => x.TodoList)
                        .FirstOrDefaultAsync(x => x.Id == id);
    }
}