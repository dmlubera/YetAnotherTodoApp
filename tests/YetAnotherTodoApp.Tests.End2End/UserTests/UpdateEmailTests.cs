using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Dummies;

namespace YetAnotherTodoApp.Tests.End2End.UserTests
{
    public class UpdateEmailTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> UpdateEmailAsync(UpdateEmailRequest request)
            => await TestClient.PutAsync("api/users/email", GetContent(request));

        private async Task<HttpResponseMessage> SignUpAsync(SignUpRequest request)
            => await TestClient.PostAsync("api/auth/sign-up", GetContent(request));

        private async Task<HttpResponseMessage> SingInAsync(SignInRequest request)
            => await TestClient.PostAsync("api/auth/sign-in", GetContent(request));

        private async Task<HttpResponseMessage> GetUserInfoAsync()
            => await TestClient.GetAsync("api/users/");

        [Fact]
        public async Task UpdateEmail_WithValidData_ReturnsOk()
        {
            var signUpRequest = new SignUpRequest
            {
                Username = "userforemailupdate",
                Email = "userforemailupdate@yetantohertodoapp.com",
                Password = "secretPassword"
            };
            var signInRequest = new SignInRequest
            {
                Email = signUpRequest.Email,
                Password = signUpRequest.Password
            };
            var updateEmailRequest = new UpdateEmailRequest
            {
                Email = "updatedEmail@yetanothertodoapp.com"
            };

            var signUpResponse = await SignUpAsync(signUpRequest);
            signUpResponse.EnsureSuccessStatusCode();

            var signInResponse = await SingInAsync(signInRequest);
            signInResponse.EnsureSuccessStatusCode();

            var jwtToken = JsonConvert.DeserializeObject<AuthSuccessResponse>(await signInResponse.Content.ReadAsStringAsync()).Token;

            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwtToken);

            var updateEmailResponse = await UpdateEmailAsync(updateEmailRequest);
            updateEmailResponse.EnsureSuccessStatusCode();

            var updatedUserInfo = await GetUserInfoAsync();
            updatedUserInfo.EnsureSuccessStatusCode();

            var content = JsonConvert.DeserializeObject<UserInfoResponse>(await updatedUserInfo.Content.ReadAsStringAsync());
            
            content.Email.Should().BeEquivalentTo(updateEmailRequest.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateEmailAsync_WithInvalidEmailFormat_ReturnsBadRequest(string email)
        {
            var expectedException = new InvalidEmailFormatException(email);
            await AuthenticateTestUserAsync();
            var request = new UpdateEmailRequest
            {
                Email = email
            };

            var response = await UpdateEmailAsync(request);
            var exception = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.Code.Should().BeEquivalentTo(expectedException.Code);
            exception.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Fact]
        public async Task UpdateEmailAsync_WithAlreadyUsedEmail_ReturnsBadRequest()
        {
            await AuthenticateTestUserAsync();
            var expectedException = new UpdateEmailToAlreadyUsedValueException();

            var request = new UpdateEmailRequest
            {
                Email = TestUser.Email
            };

            var response = await UpdateEmailAsync(request);
            var exception = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.Code.Should().BeEquivalentTo(expectedException.Code);
            exception.Message.Should().BeEquivalentTo(expectedException.Message);
        }

    }
}
