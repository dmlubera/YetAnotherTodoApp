using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Dummies;

namespace YetAnotherTodoApp.Tests.End2End.UserTests
{
    public class UpdatePasswordTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> UpdatePasswordAsync(UpdatePasswordRequest request)
            => await TestClient.PutAsync("api/users/password", GetContent(request));

        private async Task<HttpResponseMessage> SignUpAsync(SignUpRequest request)
            => await TestClient.PostAsync("api/auth/sign-up", GetContent(request));

        private async Task<HttpResponseMessage> SignInAsync(SignInRequest request)
            => await TestClient.PostAsync("api/auth/sign-in", GetContent(request));

        private async Task<HttpResponseMessage> GetUserInfoAsync()
            => await TestClient.GetAsync("api/users/");

        [Fact]
        public async Task UpdateEmail_WithValidData_ReturnsOk()
        {
            var signUpRequest = new SignUpRequest
            {
                Username = "userforpasswordupdate",
                Email = "userforpasswordupdate@yetantohertodoapp.com",
                Password = "secretPassword"
            };
            var signInRequest = new SignInRequest
            {
                Email = signUpRequest.Email,
                Password = signUpRequest.Password
            };
            var updatePasswordRequest = new UpdatePasswordRequest
            {
                Password = "superSecretPassword"
            };

            var signUpResponse = await SignUpAsync(signUpRequest);
            signUpResponse.EnsureSuccessStatusCode();

            var signInResponse = await SignInAsync(signInRequest);
            signInResponse.EnsureSuccessStatusCode();

            var jwtToken = JsonConvert.DeserializeObject<AuthSuccessResponse>(await signInResponse.Content.ReadAsStringAsync()).Token;

            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwtToken);

            var updatePasswordResponse = await UpdatePasswordAsync(updatePasswordRequest);
            updatePasswordResponse.EnsureSuccessStatusCode();

            var updatedUserInfo = await GetUserInfoAsync();
            updatedUserInfo.EnsureSuccessStatusCode();

            var invalidCredentialsResponse = await SignInAsync(signInRequest);
            invalidCredentialsResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var signInWithNewCredentialsRequest = new SignInRequest
            {
                Email = signUpRequest.Email,
                Password = updatePasswordRequest.Password
            };

            var signInWithNewCredentialsResponse = await SignInAsync(signInWithNewCredentialsRequest);
            signInWithNewCredentialsResponse.EnsureSuccessStatusCode();

            var content = JsonConvert.DeserializeObject<AuthSuccessResponse>(await signInResponse.Content.ReadAsStringAsync());
            content.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task UpdatePasswordAsync_WithAlreadyUsedPassword_ReturnsBadRequest()
        {
            await AuthenticateTestUserAsync();
            var expectedException = new UpdatePasswordToAlreadyUsedValueException();
            var request = new UpdatePasswordRequest
            {
                Password = TestUser.Password
            };

            var response = await UpdatePasswordAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}
