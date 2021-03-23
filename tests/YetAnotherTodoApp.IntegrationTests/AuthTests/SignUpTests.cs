using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.IntegrationTests.AuthTests
{
    public class SignUpTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> SignUpAsync(SignUpRequest request)
            => await TestClient.PostAsync("api/auth/sign-up", GetContent(request));

        [Fact]
        public async Task SignUpAsync_WithValidData_ReturnsCreated()
        {
            var request = new SignUpRequest
            {
                Username = "UniqueUsername",
                Email = "uniqueusername@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await SignUpAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" @yetanothertodoapp.com")]
        [InlineData("testuseryetanothertodoapp.com")]
        [InlineData("testuser@yetanothertodoapp")]
        public async Task SignUpAsync_WithInvalidEmailFormat_ReturnsBadRequest(string email)
        {
            var expectedException = new InvalidEmailFormatException(email);
            var request = new SignUpRequest
            {
                Username = "NewUniqueUsername",
                Email = email,
                Password = "secretPassword"
            };

            var response = await SignUpAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Theory]
        [InlineData("testuser@yetanothertodoapp.com")]
        [InlineData("testUser@yetanothertodoapp.com")]
        public async Task SignUpAsync_WithExistingEmail_ReturnsBadRequest(string email)
        {
            var expectedException = new EmailInUseException(email);
            var request = new SignUpRequest
            {
                Username = "NewUniqueUsername",
                Email = email,
                Password = "secretPassword"
            };

            var response = await SignUpAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("test")]
        public async Task SignUpAsync_WithInvalidUsername_ReturnsBadRequest(string username)
        {
            var expectedException = new InvalidUsernameException(username);
            var request = new SignUpRequest
            {
                Username = username,
                Email = "uniqueusername@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await SignUpAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Theory]
        [InlineData("testuser")]
        [InlineData("testUser")]
        public async Task SignUpAsync_WithExistingUsername_ReturnsBadRequest(string username)
        {
            var expectedException = new UsernameInUseException(username);
            var request = new SignUpRequest
            {
                Username = username,
                Email = "newuniqueusername@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await SignUpAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}
