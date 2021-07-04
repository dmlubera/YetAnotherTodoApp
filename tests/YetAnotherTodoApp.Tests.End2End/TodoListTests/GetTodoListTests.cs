using FluentAssertions;
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
        public async Task WithExistingId_ShouldReturnOkAndDto()
        {
            var expectedTodoList = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox");

            (var httpResponse, var todoList) =
                await HandleRequestAsync<TodoListDto>(() => ActAsync(expectedTodoList.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            todoList.Title.Should().Be(expectedTodoList.Title.Value);
        }

        [Fact]
        public async Task WithNotExisitingId_ShouldReturnRequestWithCustomError()
        {
            var id = Guid.NewGuid();
            var expectedException = new TodoListWithGivenIdDoesNotExistException(id);

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}