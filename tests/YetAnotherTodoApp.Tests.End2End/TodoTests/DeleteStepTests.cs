using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class DeleteStepTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid todoId, Guid stepId)
            => await TestClient.DeleteAsync($"api/todo/{todoId}/steps/{stepId}");

        [Fact]
        public async Task WhenStepExist_ShouldDeleteStepAndSaveChangesToDatabase()
        {
            var stepTodDelete = User.TodoLists.SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Title.Value == "TodoWithAssignedStep")
                .Steps.FirstOrDefault();

            var httpResponse = await HandleRequestAsync(() => ActAsync(stepTodDelete.Todo.Id, stepTodDelete.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todo = await DbContext.GetAsync<Todo>(stepTodDelete.Todo.Id);
            var step = await DbContext.GetAsync<Step>(stepTodDelete.Id);
            step.Should().BeNull();
            todo.Steps.Count.Should().Be(0);
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