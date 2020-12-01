using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Infrastructure.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

            builder.HasMany(x => x.TodoLists)
                .WithOne(x => x.User)
                .HasForeignKey("userId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}