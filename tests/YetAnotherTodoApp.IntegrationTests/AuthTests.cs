using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.IntegrationTests
{
    public class AuthTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> RegisterAsync(RegisterUserRequest request)
            => await TestClient.PostAsync("api/auth/register", GetContent(request));

        [Fact]
        public async Task RegiterUserAsync_WithCorrectData_ReturnsCreatedHttpStatusCode()
        {
            var request = new RegisterUserRequest
            {
                Username = "test874",
                Email = "testowy874@test.com",
                Password = "test123!@#"
            };

            var response = await RegisterAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task RegisterUserAsync_WithIncorrectEmail_ReturnsBadRequestHttpStatusCode()
        {
            var request = new RegisterUserRequest
            {
                Username = "test123",
                Email = "testowytest.com",
                Password = "test123!@#"
            };

            var response = await RegisterAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeNull();
            content.Code.Should().Be(new InvalidEmailFormatException(request.Email).Code);
        }
    }
}
