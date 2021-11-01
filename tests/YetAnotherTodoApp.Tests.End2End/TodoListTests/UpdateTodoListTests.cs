using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.TodoLists;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
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
            var todoListToUpdate = User.TodoLists.FirstOrDefault(x => x.Title == TestDbConsts.TestTodoList);
            var request = new UpdateTodoListRequest
            {
                Title = "Not important stuff"
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(todoListToUpdate.Id, request));
            var todoList = await DbContext.GetAsync<TodoList>(todoListToUpdate.Id);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            todoList.Title.Value.Should().Be(request.Title);
        }

        [Fact]
        public async Task WithInvalidData_ShouldReturnValidationError()
        {
            var todoListToUpdate = User.TodoLists.FirstOrDefault(x => x.Title == TestDbConsts.TestTodoList);

            var request = new UpdateTodoListRequest
            {
                Title = string.Empty
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(todoListToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithExistingTitle_ShouldReturnBadRequestWithCustomError()
        {
            var todoListToUpdate = User.TodoLists.FirstOrDefault(x => x.Title == TestDbConsts.TestTodoList);
            var expectedException = new TodoListWithGivenTitleAlreadyExistsException(todoListToUpdate.Title);

            var request = new UpdateTodoListRequest
            {
                Title = todoListToUpdate.Title
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(todoListToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task OnInbox_ShouldReturnBadRequestWithCustomError()
        {
            var inbox = User.TodoLists.FirstOrDefault(x => x.Title == "Inbox");
            var expectedException = new InboxModificationIsNotAllowedException();

            var request = new UpdateTodoListRequest
            {
                Title = "Not important stuff"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(inbox.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}