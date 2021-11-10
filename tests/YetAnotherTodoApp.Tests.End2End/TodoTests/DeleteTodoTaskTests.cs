using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class DeleteTodoTaskTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid todoId, Guid taskId)
            => await TestClient.DeleteAsync($"api/todo/{todoId}/tasks/{taskId}");

        [Fact]
        public async Task WhenTodoTaskExist_ShouldDeleteTodoTaskAndSaveChangesToDatabase()
        {
            var taskTodDelete = User.TodoLists
                .SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Title == TestDbConsts.TestTodo)
                .Tasks.FirstOrDefault();

            var httpResponse = await HandleRequestAsync(() => ActAsync(taskTodDelete.Todo.Id, taskTodDelete.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todo = await DbContext.GetAsync<Todo>(taskTodDelete.Todo.Id);
            var task = await DbContext.GetAsync<TodoTask>(taskTodDelete.Id);
            task.Should().BeNull();
            todo.Tasks.Count.Should().Be(0);
        }

        [Fact]
        public async Task WhenTodoDoesNotExist_ShouldReturnCustomError()
        {
            var todoId = Guid.NewGuid();
            var expectedException = new TodoWithGivenIdDoesNotExistException(todoId);

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(todoId, Guid.NewGuid()));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}