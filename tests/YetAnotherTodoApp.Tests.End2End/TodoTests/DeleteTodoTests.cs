using FluentAssertions;
using Newtonsoft.Json;
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
    public class DeleteTodoTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id)
            => await TestClient.DeleteAsync($"api/todo/{id}");

        [Fact]
        public async Task WithExistingId_ReturnsHttpStatusCodeNoContentAndRemoveTodoFromDatabase()
        {
            var todoToDelete = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox").Todos.FirstOrDefault();

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToDelete.Id);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todo = await DbContext.GetAsync<Todo>(todoToDelete.Id);
            todo.Should().BeNull();
        }

        [Fact]
        public async Task WithExistingIdOfTodoWithAssignedTasks_DeleteTodoAndAssignedTasksFromDatabase()
        {
            var todoToDelete = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault(x => x.Title.Value == "TodoWithAssignedStep");

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToDelete.Id);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todo = await DbContext.GetAsync<Todo>(todoToDelete.Id);
            todo.Should().BeNull();
            var step = await DbContext.GetAsync<Step>(todoToDelete.Steps.FirstOrDefault().Id);
            step.Should().BeNull();
        }

        [Fact]
        public async Task WithNonExistingId_ReturnHttpStatusCodeBadRequest()
        {
            var id = Guid.NewGuid();
            var expectedException = new TodoWithGivenIdDoesNotExistException(id);

            await AuthenticateTestUserAsync();
            var response = await ActAsync(id);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}
