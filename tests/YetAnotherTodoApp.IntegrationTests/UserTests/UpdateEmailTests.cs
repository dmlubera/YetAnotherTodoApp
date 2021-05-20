using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.IntegrationTests.Dummies;

namespace YetAnotherTodoApp.IntegrationTests.UserTests
{
    public class UpdateEmailTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> UpdateEmailAsync(UpdateUserEmailRequest request)
            => await TestClient.PutAsync("api/users/email", GetContent(request));

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateEmailAsync_WithInvalidEmailFormat_ReturnsBadRequest(string email)
        {
            var expectedException = new InvalidEmailFormatException(email);
            await AuthenticateTestUserAsync();
            var request = new UpdateUserEmailRequest
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

            var request = new UpdateUserEmailRequest
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
