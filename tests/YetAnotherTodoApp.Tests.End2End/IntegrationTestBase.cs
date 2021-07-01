using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api;
using YetAnotherTodoApp.Api.Models.Auths;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Infrastructure.DAL;
using YetAnotherTodoApp.Tests.End2End.Dummies;

namespace YetAnotherTodoApp.Tests.End2End
{
    public class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        protected readonly YetAnotherTodoAppDbContext DbContext;
        protected readonly HttpClient TestClient;
        protected readonly User User;

        public IntegrationTestBase()
        {
            var factory = new CustomWebApplicationFactory<Startup>();
            var providerFactory= factory.Services.GetService<IServiceScopeFactory>();
            DbContext = providerFactory.CreateScope().ServiceProvider.GetService<YetAnotherTodoAppDbContext>();
            TestClient = factory.CreateClient();
            User = factory.User;
        }

        protected static StringContent GetContent(object value)
            => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, MediaTypeNames.Application.Json);

        public async Task AuthenticateTestUserAsync()
            => TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtTokenAsync());

        private async Task<string> GetJwtTokenAsync()
        {
            var request = new SignInRequest
            {
                Email = TestUser.Email,
                Password = TestUser.Password
            };
            var response = await TestClient.PostAsync("api/auth/sign-in", GetContent(request));
            return JsonConvert.DeserializeObject<AuthSuccessResponse>(await response.Content.ReadAsStringAsync()).Token;
        }
    }
}