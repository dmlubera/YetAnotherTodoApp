using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.IntegrationTests
{
    public class AuthTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> RegisterAsync(RegisterUserRequest request)
            => await TestClient.PostAsync("api/auth/register", GetContent(request));

        private async Task<HttpResponseMessage> AuthenticateAsync(AuthenticateUserRequest request)
            => await TestClient.PostAsync("api/auth/login", GetContent(request));

        [Fact]
        public async Task RegiterUserAsync_WithCorrectData_ReturnsCreatedHttpStatusCode()
        {
            var request = new RegisterUserRequest
            {
                Username = "uniqueUsername",
                Email = "uniqueemail@test.com",
                Password = "secretPassword"
            };

            var response = await RegisterAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task RegisterUserAsync_WithIncorrectEmail_ReturnsBadRequestHttpStatusCode()
        {
            var request = new RegisterUserRequest
            {
                Username = "test1234",
                Email = "testowytest.com",
                Password = "secretPassword"
            };

            var response = await RegisterAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeNull();
            content.Code.Should().Be(new InvalidEmailFormatException(request.Email).Code);
        }

        [Fact]
        public async Task RegisterUserAsync_WithExistingEmail_ReturnsBadRequestHttpStatusCode()
        {
            var request = new RegisterUserRequest
            {
                Username = "newUniqueUsername",
                Email = "test123@test.com",
                Password = "secretPassword"
            };

            var response = await RegisterAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeNull();
            content.Code.Should().Be(new EmailInUseException(request.Email).Code);
        }

        [Fact]
        public async Task RegisterUserAsync_WithExistingUsername_ReturnsBadRequestHttpStatusCode()
        {
            var request = new RegisterUserRequest
            {
                Username = "test123",
                Email = "test1234@test.com",
                Password = "secretPassword"
            };

            var response = await RegisterAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeNull();
            content.Code.Should().Be(new UsernameInUseException(request.Username).Code);
        }

        [Fact]
        public async Task AuthenticateUserAsync_WithValidCredentials_ReturnsJwtToken()
        {
            var request = new AuthenticateUserRequest
            {
                Email = "test123@test.com",
                Password = "secretpassword"
            };

            var response = await AuthenticateAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
        }
    }
}
