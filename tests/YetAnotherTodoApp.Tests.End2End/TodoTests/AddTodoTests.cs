using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class AddTodoTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(AddTodoRequest request)
            => await TestClient.PostAsync("/api/todo", GetContent(request));

        [Fact]
        public async Task WithValidData_ReturnsHttpStatusCodeCreatedWithLocationHeaderAndSaveTodoToDatabase()
        {
            var request = new AddTodoRequest
            {
                Title = "TodoWithSpecifiedProject",
                Project = "Inbox",
                FinishDate = DateTime.UtcNow.Date
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var todoId = response.Headers.Location.GetResourceId();

            var todo = await DbContext.GetTodoWithReferencesAsync(todoId);
            todo.TodoList.Title.Value.Should().Be(request.Project);
            todo.Title.Value.Should().Be(request.Title);
            todo.FinishDate.Should().Be(request.FinishDate);
        }

        [Fact]
        public async Task WithoutProjectName_ReturnsHttpStatusCodeCreatedWithLocationHeaderAndAddToInboxAndSaveTodoToDatabase()
        {
            var request = new AddTodoRequest
            {
                Title = "TodoWithoutSpecifiedProject",
                FinishDate = DateTime.UtcNow.Date
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var todoId = response.Headers.Location.GetResourceId();

            var todo = await DbContext.GetTodoWithReferencesAsync(todoId);
            todo.TodoList.Title.Value.Should().Be("Inbox");
            todo.Title.Value.Should().Be(request.Title);
            todo.FinishDate.Should().Be(request.FinishDate);
        }

        [Fact]
        public async Task WithTasks_ReturnsHttpStatusCodeCreatedWithLocationHeaderAndSaveResourcesToDatabase()
        {
            var request = new AddTodoRequest
            {
                Title = "TodoWithActions",
                FinishDate = DateTime.UtcNow.Date,
                Steps = new []
                {
                    new StepDto { Title = "ActionOne" },
                    new StepDto { Title = "ActionTwo" }
                }
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var todoId = response.Headers.Location.GetResourceId();
            var todo = await DbContext.GetTodoWithReferencesAsync(todoId);
            todo.Title.Value.Should().Be(request.Title);
            todo.FinishDate.Should().Be(request.FinishDate);
            todo.Steps.Count.Should().Be(request.Steps.Count);
        }

        [Fact]
        public async Task WithFinishDateEarlierThanToday_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            var request = new AddTodoRequest
            {
                Title = "TodoWithInvalidData",
                FinishDate = DateTime.UtcNow.AddDays(-1).Date
            };
            var expectedException = new DateCannotBeEarlierThanTodayDateException(request.FinishDate);

            await AuthenticateTestUserAsync();
            var response = await ActAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }

        public async Task WithoutTitle_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            // should be implemented after adding FluentValidation
        }

        public async Task WithoutFinishDate_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            // should be implemented after adding FluentValidation
        }
    }
}
