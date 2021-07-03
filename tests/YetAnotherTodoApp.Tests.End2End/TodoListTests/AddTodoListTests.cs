using FluentAssertions;
using Newtonsoft.Json;
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
        private async Task<HttpResponseMessage> CreateTodoListAsync(AddTodoListRequest request)
            => await TestClient.PostAsync("api/todolist/", GetContent(request));

        [Fact]
        public async Task WithValidData_ReturnsHttpStatusCodeCreatedWithLocationHeader()
        {
            var request = new AddTodoListRequest
            {
                Title = "TestTodoList"
            };

            await AuthenticateTestUserAsync();
            var response = await CreateTodoListAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task WithValidData_AddTodoListToDatabase()
        {
            var request = new AddTodoListRequest
            {
                Title = "TestTodoList"
            };

            await AuthenticateTestUserAsync();
            var response = await CreateTodoListAsync(request);
            var resourceId = response.Headers.Location.GetResourceId();

            var todoList = await DbContext.GetAsync<TodoList>(resourceId);

            todoList.Should().NotBeNull();
            todoList.Title.Value.Should().Be(request.Title);
        }

        [Fact]
        public async Task WithInvalidData_ReturnsValidationError()
        {
            var request = new AddTodoListRequest
            {
                Title = string.Empty
            };
            
            await AuthenticateTestUserAsync();
            var response = await CreateTodoListAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithExistingTitle_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            var request = new AddTodoListRequest
            {
                Title = "Inbox"
            };
            var expectedError = new TodoListWithGivenTitleAlreadyExistsException(request.Title);

            await AuthenticateTestUserAsync();
            var response = await CreateTodoListAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().Be(expectedError.Code);
            content.Message.Should().Be(expectedError.Message);
        }
    }
}
