using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class AddTodoTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(object request)
            => await TestClient.PostAsync("/api/todo", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnCreatedAndAddResourceToDatabase()
        {
            var request = new AddTodoRequest
            {
                Title = "TodoWithSpecifiedProject",
                Project = "Inbox",
                FinishDate = DateTime.UtcNow.Date
            };

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(request);
            var todo = await DbContext.GetTodoWithReferencesAsync(httpResponse.Headers.Location.GetResourceId());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            todo.TodoList.Title.Value.Should().Be(request.Project);
            todo.Title.Value.Should().Be(request.Title);
            todo.FinishDate.Value.Should().Be(request.FinishDate);
        }

        [Fact]
        public async Task WithoutProjectName_ShuldReturnCreatedAndAddToInboxAndSaveResourceToDatabase()
        {
            var request = new AddTodoRequest
            {
                Title = "TodoWithoutSpecifiedProject",
                FinishDate = DateTime.UtcNow.Date
            };

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(request);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            httpResponse.Headers.Location.Should().NotBeNull();

            var todo = await DbContext.GetTodoWithReferencesAsync(httpResponse.Headers.Location.GetResourceId());
            todo.TodoList.Title.Value.Should().Be("Inbox");
            todo.Title.Value.Should().Be(request.Title);
            todo.FinishDate.Value.Should().Be(request.FinishDate);
        }

        [Fact]
        public async Task WithTasks_ShouldReturnCreateddSaveResourcesToDatabase()
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
            var httpResponse = await ActAsync(request);

            httpResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            httpResponse.Headers.Location.Should().NotBeNull();

            var todo = await DbContext.GetTodoWithReferencesAsync(httpResponse.Headers.Location.GetResourceId());
            todo.Title.Value.Should().Be(request.Title);
            todo.FinishDate.Value.Should().Be(request.FinishDate);
            todo.Steps.Count.Should().Be(request.Steps.Count);
        }

        [Fact]
        public async Task WithFinishDateEarlierThanToday_ShouldReturnBadRequestWithCustomError()
        {
            var request = new AddTodoRequest
            {
                Title = "TodoWithInvalidData",
                FinishDate = DateTime.UtcNow.AddDays(-1).Date
            };
            var expectedException = new DateCannotBeEarlierThanTodayDateException(request.FinishDate);

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task WithoutTitle_ShouldReturnValidationError()
        {
            var request = new
            {
                FinishDate = DateTime.UtcNow.Date
            };

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithoutFinishDate_ShouldReturnValidationError()
        {
            var request = new
            {
                Title = "test"
            };

            await AuthenticateTestUserAsync();
            var httpResponse = await ActAsync(request);
            var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await httpResponse.Content.ReadAsStringAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }
    }
}