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
                Username = "UniqueUsername",
                Email = "uniqueusername@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await RegisterAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task RegisterUserAsync_WithIncorrectEmailFormat_ReturnsBadRequestHttpStatusCode()
        {
            var request = new RegisterUserRequest
            {
                Username = "UniqueUsername",
                Email = "uniqueemailyetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await RegisterAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeNull();
            content.Code.Should().Be(new InvalidEmailFormatException(request.Email).Code);
        }

        [Theory]
        [InlineData("testuser@yetanothertodoapp.com")]
        [InlineData("testUser@yetanothertodoapp.com")]
        public async Task RegisterUserAsync_WithExistingEmail_ReturnsBadRequestHttpStatusCode(string email)
        {
            var request = new RegisterUserRequest
            {
                Username = "NewUniqueUsername",
                Email = email,
                Password = "secretPassword"
            };

            var response = await RegisterAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().NotBeNull();
            content.Code.Should().Be(new EmailInUseException(request.Email).Code);
        }

        [Theory]
        [InlineData("testuser")]
        [InlineData("testUser")]
        public async Task RegisterUserAsync_WithExistingUsername_ReturnsBadRequestHttpStatusCode(string username)
        {
            var request = new RegisterUserRequest
            {
                Username = username,
                Email = "newuniqueusername@yetanothertodoapp.com",
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
                Email = "testuser@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await AuthenticateAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().NotBeNull();
        }

        [Fact]
        public async Task AuthenticateUserAsync_WithInvalidCredentials_ReturnsBadRequestHttpStatusCode()
        {
            var request = new AuthenticateUserRequest
            {
                Email = "testuser@yetanothertodoapp.com",
                Password = "wrongSecretPassword"
            };

            var response = await AuthenticateAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            content.Should().NotBeNull();
        }

        [Fact]
        public async Task AuthenticateUserAsync_WithNotExistingUserAccount_ReturnsBadRequestHttpStatusCode()
        {
            var request = new AuthenticateUserRequest
            {
                Email = "notexistinguseraccount@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await AuthenticateAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            content.Should().NotBeNull();
        }
    }
}
