using System.Reflection;
using Microsoft.EntityFrameworkCore;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Infrastructure.DAL
{
    public class YetAnotherTodoAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Step> Steps { get; set; }

        public YetAnotherTodoAppDbContext(DbContextOptions<YetAnotherTodoAppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}