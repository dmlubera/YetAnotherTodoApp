using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Users;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Dummies;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.UserTests
{
    public class UpdatePasswordTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(UpdatePasswordRequest request)
            => await TestClient.PutAsync("api/users/password", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnOkAndUpdateUserInDatabase()
        {
            var request = new UpdatePasswordRequest
            {
                Password = "superSecretPassword"
            };

            await AuthenticateTestUserAsync();
            var httResponse = await ActAsync(request);

            httResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var user = await DbContext.GetAsync<User>(User.Id);
            var encrypter = new Encrypter();
            var hash = encrypter.GetHash(request.Password, user.Password.Salt);

            hash.Should().Be(user.Password.Hash);
        }

        [Fact]
        public async Task WithAlreadyUsedPassword_ShouldReturnBadRequestWithCustomError()
        {
            var expectedException = new UpdatePasswordToAlreadyUsedValueException();
            var request = new UpdatePasswordRequest
            {
                Password = TestUser.Password
            };

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().BeEquivalentTo(expectedException.Code);
            errorResponse.Message.Should().BeEquivalentTo(expectedException.Message);
        }
    }
}