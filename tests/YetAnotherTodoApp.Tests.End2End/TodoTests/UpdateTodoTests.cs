using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.Todos;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class UpdateTodoTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id, object request)
            => await TestClient.PutAsync($"/api/todo/{id}", GetContent(request));

        [Fact]
        public async Task WithValidData_ReturnsHttpStatusCodeOkAndUpdateTodoInDatabase()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new UpdateTodoRequest
            {
                Description = "UpdatedDescription",
                Title = "UpdatedTitle",
                FinishDate = DateTime.UtcNow.Date
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToUpdate.Id, request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var todo = await DbContext.GetAsync<Todo>(todoToUpdate.Id);
            todo.Description.Should().Be(request.Description);
            todo.Title.Value.Should().Be(request.Title);
            todo.FinishDate.Value.Should().Be(request.FinishDate);
        }

        [Fact]
        public async Task WithoutTitle_ReturnsValidationError()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new
            {
                FinishDate = DateTime.UtcNow.Date
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToUpdate.Id, request);
            var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithoutFinishDate_ReturnsValidationError()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new
            {
                Title = "test"
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToUpdate.Id, request);
            var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithEmptyTitle_ReturnsValidationError()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new UpdateTodoRequest
            {
                Description = "UpdatedDescription",
                Title = string.Empty,
                FinishDate = DateTime.UtcNow.Date
            };

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToUpdate.Id, request);
            var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithFinishDateEarlierThanToday_ReturnsHttpStatusCodeBadRequestWithCustomException()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new UpdateTodoRequest
            {
                Description = "UpdatedDescription",
                Title = "UpdatedTitle",
                FinishDate = DateTime.UtcNow.AddDays(-1).Date
            };
            var expectedException = new DateCannotBeEarlierThanTodayDateException(request.FinishDate.Date);

            await AuthenticateTestUserAsync();
            var response = await ActAsync(todoToUpdate.Id, request);
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}