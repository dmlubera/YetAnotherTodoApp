using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.IntegrationTests.Dummies;

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