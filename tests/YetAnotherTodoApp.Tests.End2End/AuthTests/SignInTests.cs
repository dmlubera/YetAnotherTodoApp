using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;

namespace YetAnotherTodoApp.Tests.End2End.AuthTests
{
    public class SignInTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> AuthenticateAsync(SignInRequest request)
            => await TestClient.PostAsync("api/auth/sign-in", GetContent(request));

        [Fact]
        public async Task SignInAsync_WithValidCredentials_ReturnsJwtToken()
        {
            var request = new SignInRequest
            {
                Email = "testuser@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await AuthenticateAsync(request);
            var content = JsonConvert.DeserializeObject<JwtDto>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task SignInAsync_WithInvalidCredentials_ReturnsBadRequest()
        {
            var expectedException = new InvalidCredentialsException();
            var request = new SignInRequest
            {
                Email = "testuser@yetanothertodoapp.com",
                Password = "wrongSecretPassword"
            };

            var response = await AuthenticateAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }

        [Fact]
        public async Task SignInAsync_WithNonExistingUserAccount_ReturnsBadRequest()
        {
            var expectedException = new InvalidCredentialsException();
            var request = new SignInRequest
            {
                Email = "notexistinguseraccount@yetanothertodoapp.com",
                Password = "secretPassword"
            };

            var response = await AuthenticateAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().BeEquivalentTo(expectedException.Code);
            content.Message.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}