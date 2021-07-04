using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Auths;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.AuthTests
{
    public class SignUpTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(SignUpRequest request)
            => await TestClient.PostAsync("api/auth/sign-up", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnCreatedAndAddResourceToDatabase()
        {
            var request = new SignUpRequest
            {
                Username = "UniqueUsername",
                Email = "uniqueusername@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(request), requireAuthentication: false);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var user = await DbContext.GetAsync<User>(httpResponse.Headers.Location.GetResourceId());
            user.Should().NotBeNull();
            user.Username.Value.Should().Be(request.Username.ToLower());
            user.Email.Value.Should().Be(request.Email.ToLower());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task WithEmptyEmail_ShouldReturnValidationError(string email)
        {
            var request = new SignUpRequest
            {
                Username = "NewUniqueUsername",
                Email = email,
                Password = "secretPassword"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(request), requireAuthentication: false);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(" @yetanothertodoapp.com")]
        [InlineData("testuseryetanothertodoapp.com")]
        [InlineData("testuser@yetanothertodoapp")]
        public async Task WithInvalidEmailFormat_ShouldReturnBadRequestWithCustomError(string email)
        {
            var expectedException = new InvalidEmailFormatException(email);
            var request = new SignUpRequest
            {
                Username = "NewUniqueUsername",
                Email = email,
                Password = "secretPassword"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(request), requireAuthentication: false);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().BeEquivalentTo(expectedException.Code);
            errorResponse.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Theory]
        [InlineData("testuser@yetanothertodoapp.com")]
        [InlineData("testUser@yetanothertodoapp.com")]
        public async Task WithExistingEmail_ShouldReturnBadRequestWithCustomError(string email)
        {
            var expectedException = new EmailInUseException(email);
            var request = new SignUpRequest
            {
                Username = "NewUniqueUsername",
                Email = email,
                Password = "secretPassword"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(request), requireAuthentication: false);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().BeEquivalentTo(expectedException.Code);
            errorResponse.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("test")]
        public async Task WithInvalidUsername_ShouldReturnValidationError(string username)
        {
            var request = new SignUpRequest
            {
                Username = username,
                Email = "uniqueusername@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(request), requireAuthentication: false);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("testuser")]
        [InlineData("testUser")]
        public async Task WithExistingUsername_ShouldReturnBadRequestWithCustomError(string username)
        {
            var expectedException = new UsernameInUseException(username);
            var request = new SignUpRequest
            {
                Username = username,
                Email = "newuniqueusername@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(request), requireAuthentication: false);


            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().BeEquivalentTo(expectedException.Code);
            errorResponse.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task WithInvalidPasswordFormat_ShouldReturnValidationError(string password)
        {
            var request = new SignUpRequest
            {
                Username = "testuser1",
                Email = "test@yetanothertodoapp.com",
                Password = password
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(request), requireAuthentication: false);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }
    }
}