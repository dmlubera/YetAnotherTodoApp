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
    public class DeleteTodoTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id)
            => await TestClient.DeleteAsync($"api/todo/{id}");

        [Fact]
        public async Task WithExistingId_ShouldReturnNoContentAndRemoveResourceFromDatabase()
        {
            var todoToDelete = User.TodoLists.FirstOrDefault(x => x.Title == "Inbox").Todos.FirstOrDefault();

            var httpResponse = await HandleRequestAsync(() => ActAsync(todoToDelete.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todo = await DbContext.GetAsync<Todo>(todoToDelete.Id);
            todo.Should().BeNull();
        }

        [Fact]
        public async Task WithExistingIdOfTodoWithAssignedTasks_ShouldReturnNoContentAndCascadeRemoveResourcesFromDatabase()
        {
            var todoToDelete = User.TodoLists
                .SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Title == TestDbConsts.TestTodo);

            var httpResponse = await HandleRequestAsync(() => ActAsync(todoToDelete.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todo = await DbContext.GetAsync<Todo>(todoToDelete.Id);
            var step = await DbContext.GetAsync<Step>(todoToDelete.Steps.FirstOrDefault().Id);
            todo.Should().BeNull();
            step.Should().BeNull();
        }

        [Fact]
        public async Task WithNonExistingId_ShouldReturnBadRequestWithCustomError()
        {
            var id = Guid.NewGuid();
            var expectedException = new TodoWithGivenIdDoesNotExistException(id);

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}