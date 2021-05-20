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
using YetAnotherTodoApp.IntegrationTests.Dummies;

namespace YetAnotherTodoApp.IntegrationTests.UserTests
{
    public class UpdatePasswordTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> UpdatePasswordAsync(UpdateUserPasswordRequest request)
            => await TestClient.PutAsync("api/users/password", GetContent(request));

        [Fact]
        public async Task UpdatePasswordAsync_WithAlreadyUsedPassword_ReturnsBadRequest()
        {
            await AuthenticateTestUserAsync();
            var expectedException = new UpdatePasswordToAlreadyUsedValueException();
            var request = new UpdateUserPasswordRequest
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
