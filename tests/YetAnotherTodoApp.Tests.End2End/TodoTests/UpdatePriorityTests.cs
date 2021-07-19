using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class UpdatePriorityTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id, object request)
            => await TestClient.PutAsync($"/api/todo/{id}/priority", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnOkAndUpdateTodoInDatabase()
        {
            var todoToUpdate = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox").Todos.FirstOrDefault();
            var request = new UpdateTodoPriorityRequest
            {
                Priority = TodoPriority.High
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var todo = await DbContext.GetAsync<Todo>(todoToUpdate.Id);
            todo.Priority.Should().Be(request.Priority);
        }

        [Fact]
        public async Task WithInvalidPriority_ShouldReturnValidationError()
        {
            var todoToUpdate = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox").Todos.FirstOrDefault();
            var request = new
            {
                Priority = "unknown"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }
    }
}