using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Auths;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;

namespace YetAnotherTodoApp.Tests.End2End.AuthTests
{
    public class SignInTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(SignInRequest request)
            => await TestClient.PostAsync("api/auth/sign-in", GetContent(request));

        [Fact]
        public async Task WithValidCredentials_ShouldReturnOkAndJwtToken()
        {
            var request = new SignInRequest
            {
                Email = "testuser@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var httpResponse = await ActAsync(request);
            var jwtResponse = JsonConvert.DeserializeObject<JwtDto>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            jwtResponse.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task WithInvalidCredentials_ShouldReturnBadRequestWithCustomError()
        {
            var expectedException = new InvalidCredentialsException();
            var request = new SignInRequest
            {
                Email = "testuser@yetanothertodoapp.com",
                Password = "wrongSecretPassword"
            };

            var httpResponse = await ActAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().BeEquivalentTo(expectedException.Code);
            errorResponse.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Fact]
        public async Task WithNonExistingUserAccount_ShouldReturnBadRequestWithCustomError()
        {
            var expectedException = new InvalidCredentialsException();
            var request = new SignInRequest
            {
                Email = "notexistinguseraccount@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var httpResponse = await ActAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().BeEquivalentTo(expectedException.Code);
            errorResponse.Message.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}