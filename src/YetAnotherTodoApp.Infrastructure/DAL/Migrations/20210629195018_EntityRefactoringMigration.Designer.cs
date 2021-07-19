﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YetAnotherTodoApp.Infrastructure.DAL;

namespace YetAnotherTodoApp.Infrastructure.DAL.Migrations
{
    [DbContext(typeof(YetAnotherTodoAppDbContext))]
    [Migration("20210629195018_EntityRefactoringMigration")]
    partial class EntityRefactoringMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.Step", b =>
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

                    b.ToTable("Steps");
                });

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

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.Step", b =>
                {
                    b.HasOne("YetAnotherTodoApp.Domain.Entities.Todo", "Todo")
                        .WithMany("Steps")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Title", "Title", b1 =>
                        {
                            b1.Property<Guid>("StepId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .HasColumnName("Title")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("StepId");

                            b1.ToTable("Steps");

                            b1.WithOwner()
                                .HasForeignKey("StepId");
                        });
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
                                .HasColumnName("FinishDate")
                                .HasColumnType("datetime2");

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
                                .HasColumnName("Title")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("TodoId");

                            b1.ToTable("Todos");

                            b1.WithOwner()
                                .HasForeignKey("TodoId");
                        });
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
                                .HasColumnName("Title")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("TodoListId");

                            b1.ToTable("TodoLists");

                            b1.WithOwner()
                                .HasForeignKey("TodoListId");
                        });
                });

            modelBuilder.Entity("YetAnotherTodoApp.Domain.Entities.User", b =>
                {
                    b.OwnsOne("YetAnotherTodoApp.Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .HasColumnName("Email")
                                .HasColumnType("nvarchar(max)");

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
                                .HasColumnName("FirstName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("LastName")
                                .HasColumnName("LastName")
                                .HasColumnType("nvarchar(max)");

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
                                .HasColumnName("PasswordHash")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Salt")
                                .HasColumnName("PasswordSalt")
                                .HasColumnType("nvarchar(max)");

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
                                .HasColumnName("Username")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}