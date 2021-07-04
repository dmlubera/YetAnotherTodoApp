using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.Users;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Dummies;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.UserTests
{
    public class UpdateUserInfoTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(UpdateUserInfoRequest request)
            => await TestClient.PutAsync($"api/users", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnOkAndUpdateUserInDatabase()
        {
            var request = new UpdateUserInfoRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await DbContext.GetAsync<User>(User.Id);

            user.Email.Value.Should().BeEquivalentTo(TestUser.Email);
            user.Username.Value.Should().BeEquivalentTo(TestUser.Username);
            user.Name.FirstName.Should().BeEquivalentTo("John");
            user.Name.LastName.Should().BeEquivalentTo("Doe");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task WithInvalidFirstName_ShouldReturnValidationError(string firstName)
        {
            var request = new UpdateUserInfoRequest
            {
                FirstName = firstName,
                LastName = "Doe"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task WithInvalidLastName_ShouldReturnValidationError(string lastName)
        {
            var request = new UpdateUserInfoRequest
            {
                FirstName = "John",
                LastName = lastName
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }
    }
}