using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api;
using YetAnotherTodoApp.Infrastructure.DAL;

namespace YetAnotherTodoApp.IntegrationTests
{
    public class IntegrationTestBase
    {
        protected readonly HttpClient TestClient;

        public IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<YetAnotherTodoAppDbContext>));
                        if (descriptor != null)
                            services.Remove(descriptor);

                        services.AddDbContext<YetAnotherTodoAppDbContext>(opts => opts.UseInMemoryDatabase("TestDb"));
                    });
                });
            TestClient = appFactory.CreateClient();
        }

        protected static StringContent GetContent(object value)
            => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}