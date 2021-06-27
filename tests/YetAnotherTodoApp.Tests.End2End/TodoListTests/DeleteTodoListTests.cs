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
using YetAnotherTodoApp.Tests.End2End.Dummies;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoListTests
{
    public class DeleteTodoListTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id)
            => await TestClient.DeleteAsync($"/api/todolist/{id}");

        [Fact]
        public async Task WithExistingId_ReturnsHttpStatusCodeNoContentAndRemoveTodoListFromDatabase()
        {
            var resourceToRemove = User.TodoLists.FirstOrDefault(x => x.Title.Value == TestTodoList.Title);

            await AuthenticateTestUserAsync();
            var response = await ActAsync(resourceToRemove.Id);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todoList = await DbContext.GetAsync<TodoList>(resourceToRemove.Id);

            todoList.Should().BeNull();
        }

        [Fact]
        public async Task WithExistingIdOfTodoListWithTodoWithAssignedTasks_DeleteTodoListWithAssignedTodoAndTasksFromDatabse()
        {
            var resourceToRemove = User.TodoLists.FirstOrDefault(x => x.Title.Value == TodoListWithAssignedTodo.Title);
            
            await AuthenticateTestUserAsync();
            var response = await ActAsync(resourceToRemove.Id);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todoList = await DbContext.GetAsync<TodoList>(resourceToRemove.Id);
            var todo = await DbContext.GetAsync<Todo>(resourceToRemove.Todos.FirstOrDefault().Id);

            todoList.Should().BeNull();
            todo.Should().BeNull();
        }

        [Fact]
        public async Task WithNonExistingId_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            var id = Guid.NewGuid();
            var expectedException = new TodoListWithGivenIdDoesNotExistException(id);
            
            await AuthenticateTestUserAsync();
            var response = await ActAsync(id);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}