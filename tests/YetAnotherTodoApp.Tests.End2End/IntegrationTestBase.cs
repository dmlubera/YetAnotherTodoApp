using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
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

        protected async Task AuthenticateTestUserAsync()
            => TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtTokenAsync());

        private async Task<string> GetJwtTokenAsync()
        {
            var request = new SignInRequest
            {
                Email = TestDbConsts.TestUserEmail,
                Password = TestDbConsts.TestUserPassword
            };
            var response = await TestClient.PostAsync("api/auth/sign-in", GetContent(request));
            return JsonConvert.DeserializeObject<AuthSuccessResponse>(await response.Content.ReadAsStringAsync()).Token;
        }

        protected async Task<HttpResponseMessage> HandleRequestAsync(Func<Task<HttpResponseMessage>> func, bool requireAuthentication = true)
        {
            if (requireAuthentication)
                await AuthenticateTestUserAsync();

            return await func();
        }

        protected async Task<(HttpResponseMessage httpResponseMessage, TResult responseContent)> HandleRequestAsync<TResult>(Func<Task<HttpResponseMessage>> func, bool requireAuthentication = true)
            where TResult : class
        {
            if (requireAuthentication)
                await AuthenticateTestUserAsync();

            var httpResponse = await func();
            var responseContent = JsonConvert.DeserializeObject<TResult>(await httpResponse.Content.ReadAsStringAsync());

            return (httpResponse, responseContent);
        }
    }
}