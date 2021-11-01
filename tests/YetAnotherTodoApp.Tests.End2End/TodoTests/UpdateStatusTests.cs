using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class UpdateStatusTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id, object request)
            => await TestClient.PutAsync($"/api/todo/{id}/status", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnOkAndUpdateTodoInDatabase()
        {
            var todoToUpdate = User.TodoLists.FirstOrDefault(x => x.Title == "Inbox").Todos.FirstOrDefault();
            var request = new UpdateTodoStatusRequest
            {
                Status = TodoStatus.InProgress
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var todo = await DbContext.GetAsync<Todo>(todoToUpdate.Id);
            todo.Status.Should().Be(request.Status);
        }

        [Fact]
        public async Task WithInvalidStatus_ShouldReturnValidationError()
        {
            var todoToUpdate = User.TodoLists.FirstOrDefault(x => x.Title == "Inbox").Todos.FirstOrDefault();
            var request = new
            {
                Status = "unknown"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(todoToUpdate.Id, request));
            
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ToDoneWhenTodoHasUnfinishedSteps_ShouldReturnCustomError()
        {
            var todoToUpdate = User.TodoLists
                .SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Title == TestDbConsts.TestTodo);
            var expectedException = new CannotChangeStatusToDoneOfTodoWithUnfinishedStepException();
            var request = new UpdateTodoStatusRequest
            {
                Status = TodoStatus.Done
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}