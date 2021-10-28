using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using YetAnotherTodoApp.Api;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Infrastructure.DAL;

namespace YetAnotherTodoApp.Tests.Behavior
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
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
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<YetAnotherTodoAppDbContext>();
                dbContext.Database.EnsureCreated();
                InitializeDatabase(dbContext);
            });
        }

        private static void InitializeDatabase(YetAnotherTodoAppDbContext dbContext)
        {
            var user = CreateUser("administrator", "admin@yetanothertodoapp.com", "super$ecret");
            dbContext.Users.Add(user);
            dbContext.SaveChangesAsync();
        }

        private static User CreateUser(string username, string email, string password)
        {
            var encrypter = new Encrypter();
            var salt = encrypter.GetSalt();
            var hash = encrypter.GetHash(password, salt);

            return new User(username, email, hash, salt);
        }
    }
}