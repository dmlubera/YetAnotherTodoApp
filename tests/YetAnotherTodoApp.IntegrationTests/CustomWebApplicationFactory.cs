using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Infrastructure.DAL;
using YetAnotherTodoApp.IntegrationTests.Dummies;

namespace YetAnotherTodoApp.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<YetAnotherTodoAppDbContext>));
                services.Remove(descriptor);
                services.AddDbContext<YetAnotherTodoAppDbContext>(opts => opts.UseInMemoryDatabase("InMemoryDb"));

                var serviceProvider = services.BuildServiceProvider();
                using(var scope = serviceProvider.CreateScope())
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
            CreateInitialUserAccount(dbContext);
        }

        private void CreateInitialUserAccount(YetAnotherTodoAppDbContext dbContext)
        {
            var encrypter = new Encrypter();
            var salt = encrypter.GetSalt();
            var password = encrypter.GetHash(TestUser.Password, salt);
            dbContext.Users.Add(new User(TestUser.Username, TestUser.Email, password, salt));
            dbContext.SaveChanges();
        }
    }
}
