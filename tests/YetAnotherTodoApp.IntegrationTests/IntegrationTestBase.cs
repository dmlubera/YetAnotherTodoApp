using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Xunit;
using YetAnotherTodoApp.Api;

namespace YetAnotherTodoApp.IntegrationTests
{
    public class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        protected readonly HttpClient TestClient;

        public IntegrationTestBase()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            TestClient = _factory.CreateClient();
        }

        protected static StringContent GetContent(object value)
            => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}