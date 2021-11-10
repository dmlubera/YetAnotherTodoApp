using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.TodoTasks;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class UpdateTodoTaskTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid taskId, object request)
            => await TestClient.PutAsync($"api/todo/tasks/{taskId}", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnOkAndUpdateTodoTaskInDatabase()
        {
            var taskToUpdate = User.TodoLists
                .SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Title == TestDbConsts.TestTodo)
                .Tasks.FirstOrDefault();
            var request = new UpdateTodoTaskRequest
            {
                Title = "UpdatedTitle",
                Description = "UpdatedDescription",
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(taskToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var todo = await DbContext.GetAsync<TodoTask>(taskToUpdate.Id);
            todo.Title.Value.Should().Be(request.Title);
            todo.Description.Should().Be(request.Description);
        }

        [Fact]
        public async Task WhenTodoTaskDoesNotExist_ShouldReturnCustomErrorCode()
        {
            var taskId = Guid.NewGuid();
            var expectedException = new TodoTaskWithGivenIdDoesNotExistException(taskId);
            var request = new UpdateTodoTaskRequest
            {
                Title = "UpdatedTitle",
                Description = "UpdatedDescription",
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(taskId, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task WithoutTitle_ShouldReturnValidationError()
        {
            var request = new
            {
                Description = "UpdatedDescription"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(Guid.NewGuid(), request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithEmptyTitle_ShouldReturnValidationError()
        {
            var request = new UpdateTodoTaskRequest
            {
                Title = string.Empty,
                Description = "UpdatedDescription",
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(Guid.NewGuid(), request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }
    }
}