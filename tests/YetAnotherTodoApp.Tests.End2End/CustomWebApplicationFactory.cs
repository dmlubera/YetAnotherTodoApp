using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Infrastructure.DAL;

namespace YetAnotherTodoApp.Tests.End2End
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public User User { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<YetAnotherTodoAppDbContext>));
                services.Remove(descriptor);
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                services.AddDbContext<YetAnotherTodoAppDbContext>(opts => opts.UseSqlite(connection));

                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<YetAnotherTodoAppDbContext>();
                    db.Database.EnsureCreated();
                    InitializeDbForTests(db);
                }
            });
        }

        private void InitializeDbForTests(YetAnotherTodoAppDbContext dbContext)
        {
            User = CreateTestUser(TestDbConsts.TestUserUsername, TestDbConsts.TestUserEmail, TestDbConsts.TestUserPassword);
            User.TodoLists.FirstOrDefault(x => x.Title == "Inbox")
                .AddTodo(CreateTestTodo(TestDbConsts.TestTodo));

            User.AddTodoList(new TodoList(TestDbConsts.TestTodoList));
            User.AddTodoList(CreateTodoListWithAssignedTodo(TestDbConsts.TestTodoListWithAssignedTodo));

            dbContext.Users.Add(User);
            dbContext.Users.Add(CreateTestUser("secondTestUser", "secondTestUser@yetanothertodoapp.com", "superSecretPassword"));
            dbContext.SaveChanges();
        }

        private static User CreateTestUser(string username, string email, string password)
        {
            var encrypter = new Encrypter();
            var salt = encrypter.GetSalt();
            var hash = encrypter.GetHash(password, salt);

            return new User(username, email, hash, salt);
        }

        private static Todo CreateTestTodo(string title)
        {
            var todo = new Todo(title, DateTime.UtcNow.Date);
            todo.AddSteps(new[] { new Step("First") });

            return todo;
        }

        private static TodoList CreateTodoListWithAssignedTodo(string title)
        {
            var todoList = new TodoList(title);
            todoList.AddTodo(new Todo("Assigned", DateTime.UtcNow.Date));

            return todoList;
        }
    }
}