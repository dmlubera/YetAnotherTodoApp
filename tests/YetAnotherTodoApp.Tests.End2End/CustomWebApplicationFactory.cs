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
using YetAnotherTodoApp.Tests.End2End.Dummies;

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
            var encrypter = new Encrypter();
            var salt = encrypter.GetSalt();
            var password = encrypter.GetHash(TestUser.Password, salt);
            var user = new User(TestUser.Username, TestUser.Email, password, salt);
            var todoList = new TodoList(TestTodoList.Title);
            var todoListWithAssignedTodo = new TodoList(TodoListWithAssignedTodo.Title);
            todoListWithAssignedTodo.AddTodo(new Todo("AssignedTestTodo", DateTime.UtcNow.Date));
            
            user.AddTodoList(todoList);
            user.AddTodoList(todoListWithAssignedTodo);
            User = user;
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
    }
}