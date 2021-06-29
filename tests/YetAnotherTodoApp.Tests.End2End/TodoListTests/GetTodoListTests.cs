using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;

namespace YetAnotherTodoApp.Tests.End2End.TodoListTests
{
    public class GetTodoListTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id)
            => await TestClient.GetAsync($"/api/todolist/{id}");

        [Fact]
        public async Task WithExistingId_ReturnsHttpStatusCodeOkAndDto()
        {
            var expectedTodoList = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox");

            await AuthenticateTestUserAsync();
            var response = await ActAsync(expectedTodoList.Id);
            var retrievedResource = JsonConvert.DeserializeObject<TodoListDto>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            retrievedResource.Title.Should().Be(expectedTodoList.Title.Value);
        }

        [Fact]
        public async Task WithNonExisitingId_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            var resourceId = Guid.NewGuid();
            var expectedException = new TodoListWithGivenIdDoesNotExistException(resourceId);

            await AuthenticateTestUserAsync();
            var response = await ActAsync(resourceId);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}