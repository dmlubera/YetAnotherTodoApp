using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.TodoLists;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Dummies;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoListTests
{
    public class UpdateTodoListTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id, UpdateTodoListRequest request)
            => await TestClient.PutAsync($"/api/todolist/{id}", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnOkAndUpdateResourceInDatabase()
        {
            var todoListToUpdate = User.TodoLists.FirstOrDefault(x => x.Title.Value == TodoListForUpdateTests.Title);
            var request = new UpdateTodoListRequest
            {
                Title = "UpdatedTitle"
            };

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(todoListToUpdate.Id, request);
            var todoList = await DbContext.GetAsync<TodoList>(todoListToUpdate.Id);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            todoList.Title.Value.Should().Be(request.Title);
        }

        [Fact]
        public async Task WithInvalidData_ShouldReturnValidationError()
        {
            var todoListToUpdate = User.TodoLists.FirstOrDefault(x => x.Title.Value == TodoListForUpdateTests.Title);

            var request = new UpdateTodoListRequest
            {
                Title = string.Empty
            };

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(todoListToUpdate.Id, request);
            var errorReponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorReponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithExistingTitle_ShouldReturnBadRequestWithCustomError()
        {
            var todoListToUpdate = User.TodoLists.FirstOrDefault(x => x.Title.Value == TodoListForUpdateTests.Title);
            var expectedException = new Application.Exceptions.TodoListWithGivenTitleAlreadyExistsException(todoListToUpdate.Title.Value);

            var request = new UpdateTodoListRequest
            {
                Title = todoListToUpdate.Title.Value
            };

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(todoListToUpdate.Id, request);
            var errorReponse = JsonConvert.DeserializeObject<ErrorResponse>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorReponse.Code.Should().Be(expectedException.Code);
            errorReponse.Message.Should().Be(expectedException.Message);
        }
    }
}