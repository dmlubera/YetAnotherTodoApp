using Microsoft.EntityFrameworkCore;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Infrastructure.DAL
{
    public class YetAnotherTodoAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public YetAnotherTodoAppDbContext(DbContextOptions<YetAnotherTodoAppDbContext> options)
            : base(options)
        {
            
        }
    }
}