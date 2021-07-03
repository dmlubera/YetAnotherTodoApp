using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.Users;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Dummies;

namespace YetAnotherTodoApp.Tests.End2End.UserTests
{
    public class UpdateUserInfoTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> UpdateUserInfoAsync(UpdateUserInfoRequest request)
            => await TestClient.PutAsync($"api/users", GetContent(request));

        private async Task<HttpResponseMessage> GetUserInfoAsync()
            => await TestClient.GetAsync($"api/users");

        [Fact]
        public async Task UpdateUserInfo_WithValidData_ShouldUpdateUserInfo()
        {
            await AuthenticateTestUserAsync();

            var request = new UpdateUserInfoRequest
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var updateUserInfoResponse = await UpdateUserInfoAsync(request);
            updateUserInfoResponse.EnsureSuccessStatusCode();

            var userInfoResponse = await GetUserInfoAsync();
            var updatedUserInfo = JsonConvert.DeserializeObject<UserInfoResponse>(await userInfoResponse.Content.ReadAsStringAsync());

            updatedUserInfo.Email.Should().BeEquivalentTo(TestUser.Email);
            updatedUserInfo.Username.Should().BeEquivalentTo(TestUser.Username);
            updatedUserInfo.FirstName.Should().BeEquivalentTo("John");
            updatedUserInfo.LastName.Should().BeEquivalentTo("Doe");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateUserInfo_WithInvalidFirstName_ShouldReturnValidationError(string firstName)
        {
            await AuthenticateTestUserAsync();

            var request = new UpdateUserInfoRequest
            {
                FirstName = firstName,
                LastName = "Doe"
            };

            var response = await UpdateUserInfoAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateUserInfo_WithInvalidLastName_ShouldThrowAnException(string lastName)
        {
            await AuthenticateTestUserAsync();

            var request = new UpdateUserInfoRequest
            {
                FirstName = "John",
                LastName = lastName
            };

            var response = await UpdateUserInfoAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }
    }
}
