using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class UpdateStatusTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id, UpdateTodoStatusRequest request)
            => await TestClient.PutAsync($"/api/todo/{id}/status", GetContent(request));

        [Fact]
        public async Task WithValidData_ReturnsHttpStatusCodeOkAndUpdateTodoInDatabase()
        {
            var todoToUpdate = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox").Todos.FirstOrDefault();
            var request = new UpdateTodoStatusRequest
            {
                Status = Domain.Enums.TodoStatus.InProgress
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToUpdate.Id, request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var todo = await DbContext.GetAsync<Todo>(todoToUpdate.Id);
            todo.Status.Should().Be(request.Status);
        }

        public async Task WithInvalidStatus_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {

        }
    }
}
