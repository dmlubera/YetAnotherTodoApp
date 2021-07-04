using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.Users;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Dummies;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.UserTests
{
    public class UpdateEmailTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(UpdateEmailRequest request)
            => await TestClient.PutAsync("api/users/email", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnOkAndUpdateUserInfoInDatabase()
        {
            var request = new UpdateEmailRequest
            {
                Email = "updatedEmail@yetanothertodoapp.com"
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await DbContext.GetAsync<User>(User.Id);
            user.Email.Value.Should().BeEquivalentTo(request.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task WithInvalidEmailFormat_ShouldReturnValidationError(string email)
        {
            var request = new UpdateEmailRequest
            {
                Email = email
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithAlreadyUsedEmail_ShouldReturnBadRequestWithCustomError()
        {
            var expectedException = new UpdateEmailToAlreadyUsedValueException();

            var request = new UpdateEmailRequest
            {
                Email = TestUser.Email
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().BeEquivalentTo(expectedException.Code);
            errorResponse.Message.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}