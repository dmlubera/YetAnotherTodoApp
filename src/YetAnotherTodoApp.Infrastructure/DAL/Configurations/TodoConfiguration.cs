﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Infrastructure.DAL.Configurations
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

            builder.HasMany(x => x.Steps)
                .WithOne(x => x.Todo)
                .HasForeignKey("TodoId")
                .OnDelete(DeleteBehavior.Cascade);

            // Value Objects
            builder.OwnsOne(x => x.Title).Property(x => x.Value).HasColumnName("Title");
            builder.OwnsOne(x => x.FinishDate).Property(x => x.Value).HasColumnName("FinishDate");
        }
    }
}