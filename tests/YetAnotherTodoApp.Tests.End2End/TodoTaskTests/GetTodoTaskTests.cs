using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;

namespace YetAnotherTodoApp.Tests.End2End.TodoTaskTests
{
    public class GetTodoTaskTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id)
            => await TestClient.GetAsync($"/api/todoTasks/{id}");

        [Fact]
        public async Task WithExistingId_ShouldReturnsHttpStatusCodeOkAndDto()
        {
            var expectedTodoTask = User.TodoLists.FirstOrDefault(x => x.Title == "Inbox")
                .Todos.FirstOrDefault()
                .Tasks.FirstOrDefault();

            await AuthenticateTestUserAsync();
            (var httpResponse, var todo) =
                await HandleRequestAsync<TodoTaskDto>(() => ActAsync(expectedTodoTask.Id));

            todo.Title.Should().Be(expectedTodoTask.Title);
            todo.Description.Should().Be(expectedTodoTask.Description);
            todo.IsFinished.Should().Be(expectedTodoTask.IsFinished);
        }

        [Fact]
        public async Task WithNonExistingId_ShouldReturnBadRequestWithCustomError()
        {
            var id = Guid.NewGuid();
            var expectedException = new TodoTaskWithGivenIdDoesNotExistException(id);

            await AuthenticateTestUserAsync();
            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}