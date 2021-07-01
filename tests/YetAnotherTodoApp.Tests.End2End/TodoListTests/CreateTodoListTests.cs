using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.TodoLists;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoListTests
{
    public class CreateTodoListTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> CreateTodoListAsync(CreateTodoListRequest request)
            => await TestClient.PostAsync("api/todolist/", GetContent(request));

        [Fact]
        public async Task WithValidData_ReturnsHttpStatusCodeCreatedWithLocationHeader()
        {
            var request = new CreateTodoListRequest
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
            var request = new CreateTodoListRequest
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
        public async Task WithInvalidData_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            var request = new CreateTodoListRequest
            {
                Title = string.Empty
            };
            var expectedError = new InvalidTitleException(request.Title);

            await AuthenticateTestUserAsync();
            var response = await CreateTodoListAsync(request);
            var content = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Code.Should().Be(expectedError.Code);
            content.Message.Should().Be(expectedError.Message);
        }

        [Fact]
        public async Task WithExistingTitle_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            var request = new CreateTodoListRequest
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
