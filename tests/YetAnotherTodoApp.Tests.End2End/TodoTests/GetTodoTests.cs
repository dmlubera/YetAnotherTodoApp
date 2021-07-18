using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
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
        public async Task WithExistingId_ShouldReturnsHttpStatusCodeOkAndDto()
        {
            var expectedTodo = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox").Todos.FirstOrDefault();

            await AuthenticateTestUserAsync();
            (var httpResponse, var todo) =
                await HandleRequestAsync<TodoDto>(() => ActAsync(expectedTodo.Id));

            todo.Title.Should().Be(expectedTodo.Title.Value);
            todo.FinishDate.Should().Be(expectedTodo.FinishDate.Value);
        }

        [Fact]
        public async Task WithNonExistingId_ShouldReturnBadRequestWithCustomError()
        {
            var id = Guid.NewGuid();
            var expectedException = new TodoWithGivenIdDoesNotExistException(id);

            await AuthenticateTestUserAsync();
            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}