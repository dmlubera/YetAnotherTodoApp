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
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade);
            
            // ValueObjects
            builder.OwnsOne(x => x.Username).Property(x => x.Value).HasColumnName("Username");
            builder.OwnsOne(x => x.Name).Property(x => x.FirstName).HasColumnName("FirstName");
            builder.OwnsOne(x => x.Name).Property(x => x.LastName).HasColumnName("LastName");
            builder.OwnsOne(x => x.Email).Property(x => x.Value).HasColumnName("Email");
            builder.OwnsOne(x => x.Password).Property(x => x.Hash).HasColumnName("PasswordHash");
            builder.OwnsOne(x => x.Password).Property(x => x.Salt).HasColumnName("PasswordSalt");
        }
    }
}