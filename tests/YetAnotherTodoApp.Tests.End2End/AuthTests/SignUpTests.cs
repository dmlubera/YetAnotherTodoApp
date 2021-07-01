using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Auths;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.AuthTests
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

        [Fact]
        public async Task SignUpAsync_WithValidData_AddsUserToDatabase()
        {
            var request = new SignUpRequest
            {
                Username = "YetAnotherUniqueUsername",
                Email = "yetanotheruniqueusername@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await SignUpAsync(request);
            
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var locationHeader = response.Headers.Location;
            var userId = locationHeader.ToString().Split('/').Last();

            var user = await DbContext.GetAsync<User>(Guid.Parse(userId));

            user.Should().NotBeNull();
            user.Username.Value.Should().Be(request.Username.ToLower());
            user.Email.Value.Should().Be(request.Email.ToLower());
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task SignUpAsync_WithInvalidPasswordFormat_ReturnsBadRequest(string password)
        {
            var expectedException = new InvalidPasswordFormatException();
            var request = new SignUpRequest
            {
                Username = "testuser1",
                Email = "test@yetanothertodoapp.com",
                Password = password
            };

            var response = await SignUpAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());
            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}
