using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class GetTodoTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id)
            => await TestClient.GetAsync($"/api/todo/{id}");

        [Fact]
        public async Task WithExistingId_ReturnsHttpStatusCodeOkAndDto()
        {
            var expectedTodo = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox").Todos.FirstOrDefault();

            await AuthenticateTestUserAsync();
            var response = await ActAsync(expectedTodo.Id);
            var todo = JsonConvert.DeserializeObject<TodoDto>(await response.Content.ReadAsStringAsync());

            todo.Title.Should().Be(expectedTodo.Title.Value);
            todo.FinishDate.Should().Be(expectedTodo.FinishDate.Date);
        }

        [Fact]
        public async Task WithNonExistingId_ReturnHttpStatusCodeBadRequestWithCustomException()
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