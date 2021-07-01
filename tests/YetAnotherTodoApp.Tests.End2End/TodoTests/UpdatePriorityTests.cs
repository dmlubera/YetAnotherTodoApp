using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class UpdatePriorityTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id, UpdateTodoPriorityRequest request)
            => await TestClient.PutAsync($"/api/todo/{id}/priority", GetContent(request));

        [Fact]
        public async Task WithValidData_ReturnsHttpStatusCodeOkAndUpdateTodoInDatabase()
        {
            var todoToUpdate = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox").Todos.FirstOrDefault();
            var request = new UpdateTodoPriorityRequest
            {
                Priority = TodoPriority.High
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToUpdate.Id, request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var todo = await DbContext.GetAsync<Todo>(todoToUpdate.Id);
            todo.Priority.Should().Be(request.Priority);
        }

        public async Task WithInvalidPriority_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            // should be implemented after adding FluentValidation
        }
    }
}