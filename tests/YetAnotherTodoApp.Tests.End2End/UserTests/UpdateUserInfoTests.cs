using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
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
        public async Task UpdateUserInfo_WithInvalidFirstName_ShouldThrowAnException(string firstName)
        {
            var expectedException = new InvalidFirstNameException(firstName);
            await AuthenticateTestUserAsync();

            var request = new UpdateUserInfoRequest
            {
                FirstName = firstName,
                LastName = "Doe"
            };

            var response = await UpdateUserInfoAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateUserInfo_WithInvalidLastName_ShouldThrowAnException(string lastName)
        {
            var expectedException = new InvalidLastNameException(lastName);
            await AuthenticateTestUserAsync();

            var request = new UpdateUserInfoRequest
            {
                FirstName = "John",
                LastName = lastName
            };

            var response = await UpdateUserInfoAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}
