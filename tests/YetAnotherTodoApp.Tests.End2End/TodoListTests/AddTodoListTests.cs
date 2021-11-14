using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.TodoLists;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoListTests
{
    public class AddTodoListTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(AddTodoListRequest request)
            => await TestClient.PostAsync("api/todolists/", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnCreatedAndAddResourceToDatabase()
        {
            var request = new AddTodoListRequest
            {
                Title = "Work stuff"
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            httpResponse.Headers.Location.Should().NotBeNull();

            var todoList = await DbContext.GetAsync<TodoList>(httpResponse.Headers.Location.GetResourceId());
            todoList.Should().NotBeNull();
            todoList.Title.Value.Should().Be(request.Title);
        }

        [Fact]
        public async Task WithInvalidData_ShouldReturnValidationError()
        {
            var request = new AddTodoListRequest
            {
                Title = string.Empty
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithExistingTitle_ShouldReturnBadRequestWithCustomError()
        {
            var request = new AddTodoListRequest
            {
                Title = "Inbox"
            };
            var expectedError = new TodoListWithGivenTitleAlreadyExistsException(request.Title);

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedError.Code);
            errorResponse.Message.Should().Be(expectedError.Message);
        }
    }
}