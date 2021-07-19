using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Infrastructure.DAL.Configurations
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

            builder.HasMany(x => x.Todos)
                .WithOne(x => x.TodoList)
                .HasForeignKey("TodoListId")
                .OnDelete(DeleteBehavior.Cascade);

            // Value Objects
            builder.OwnsOne(x => x.Title).Property(x => x.Value).HasColumnName("Title");
        }
    }
}