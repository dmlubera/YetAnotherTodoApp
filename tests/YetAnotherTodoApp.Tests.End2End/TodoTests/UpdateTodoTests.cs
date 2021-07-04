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
        public async Task WithValidData_ShouldReturnOkAndUpdateTodoInDatabase()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new UpdateTodoRequest
            {
                Description = "UpdatedDescription",
                Title = "UpdatedTitle",
                FinishDate = DateTime.UtcNow.Date
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var todo = await DbContext.GetAsync<Todo>(todoToUpdate.Id);
            todo.Description.Should().Be(request.Description);
            todo.Title.Value.Should().Be(request.Title);
            todo.FinishDate.Value.Should().Be(request.FinishDate);
        }

        [Fact]
        public async Task WithoutTitle_ShouldReturnValidationError()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new
            {
                FinishDate = DateTime.UtcNow.Date
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithoutFinishDate_ShouldReturnValidationError()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new
            {
                Title = "test"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithEmptyTitle_ShouldReturnValidationError()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new UpdateTodoRequest
            {
                Description = "UpdatedDescription",
                Title = string.Empty,
                FinishDate = DateTime.UtcNow.Date
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithFinishDateEarlierThanToday_ShouldReturnBadRequestWithCustomError()
        {
            var todoToUpdate = User.TodoLists.SelectMany(x => x.Todos).FirstOrDefault();
            var request = new UpdateTodoRequest
            {
                Description = "UpdatedDescription",
                Title = "UpdatedTitle",
                FinishDate = DateTime.UtcNow.AddDays(-1).Date
            };
            var expectedException = new DateCannotBeEarlierThanTodayDateException(request.FinishDate.Date);

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(todoToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}