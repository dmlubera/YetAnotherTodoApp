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

namespace YetAnotherTodoApp.Tests.End2End.TodoTaskTests
{
    public class CompleteTodoTaskTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid taskId)
            => await TestClient.PutAsync($"api/todoTasks/{taskId}/complete", null);

        [Fact]
        public async Task WhenTodoTaskExist_ShouldMarkTaskAsCompletedAndSaveChangesToDatabase()
        {
            var taskToComplete = User.TodoLists
                .SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Title == TestDbConsts.TestTodo)
                .Tasks.FirstOrDefault();

            var httpResponse = await HandleRequestAsync(() => ActAsync(taskToComplete.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var task = await DbContext.GetAsync<TodoTask>(taskToComplete.Id);
            task.IsFinished.Should().Be(true);
        }

        [Fact]
        public async Task WhenTodoTaskDoesNotExist_ShouldReturnCustomError()
        {
            var id = Guid.NewGuid();
            var expectedException = new TodoTaskWithGivenIdDoesNotExistException(id); 

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }

    }
}