﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YetAnotherTodoApp.Infrastructure.DAL;

namespace YetAnotherTodoApp.Infrastructure.DAL.Migrations
{
    [DbContext(typeof(YetAnotherTodoAppDbContext))]
    partial class YetAnotherTodoAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.Todo", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid?>("TodoListId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TodoListId");

                    b.ToTable("Todos");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.TodoList", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.TodoTask", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("TodoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TodoId");

                    b.ToTable("TodoTasks");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.Todo", b =>
                {
                    b.HasOne("YetAnotherTodoApp.Domain.Entities.TodoList", "TodoList")
                        .WithMany("Todos")
                        .HasForeignKey("TodoListId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.FinishDate", "FinishDate", b1 =>
                        {
                            b1.Property<Guid>("TodoId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTime>("Value")
                                .HasColumnType("datetime2")
                                .HasColumnName("FinishDate");

                            b1.HasKey("TodoId");

                            b1.ToTable("Todos");

                            b1.WithOwner()
                                .HasForeignKey("TodoId");
                        });

                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Title", "Title", b1 =>
                        {
                            b1.Property<Guid>("TodoId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Title");

                            b1.HasKey("TodoId");

                            b1.ToTable("Todos");

                            b1.WithOwner()
                                .HasForeignKey("TodoId");
                        });

                    b.Navigation("FinishDate");

                    b.Navigation("Title");

                    b.Navigation("TodoList");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.TodoList", b =>
                {
                    b.HasOne("YetAnotherTodoApp.Domain.Entities.User", "User")
                        .WithMany("TodoLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Title", "Title", b1 =>
                        {
                            b1.Property<Guid>("TodoListId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Title");

                            b1.HasKey("TodoListId");

                            b1.ToTable("TodoLists");

                            b1.WithOwner()
                                .HasForeignKey("TodoListId");
                        });

                    b.Navigation("Title");

                    b.Navigation("User");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.TodoTask", b =>
                {
                    b.HasOne("YetAnotherTodoApp.Domain.Entities.Todo", "Todo")
                        .WithMany("Tasks")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Title", "Title", b1 =>
                        {
                            b1.Property<Guid>("TodoTaskId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Title");

                            b1.HasKey("TodoTaskId");

                            b1.ToTable("TodoTasks");

                            b1.WithOwner()
                                .HasForeignKey("TodoTaskId");
                        });

                    b.Navigation("Title");

                    b.Navigation("Todo");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.User", b =>
                {
                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Email");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Name", "Name", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("FirstName")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("LastName")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("LastName");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Password", "Password", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Hash")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("PasswordHash");

                            b1.Property<string>("Salt")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("PasswordSalt");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Username", "Username", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Username");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Email");

                    b.Navigation("Name");

                    b.Navigation("Password");

                    b.Navigation("Username");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.Todo", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.TodoList", b =>
                {
                    b.Navigation("Todos");
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.User", b =>
                {
                    b.Navigation("TodoLists");
                });
#pragma warning restore 612, 618
        }
    }
}
