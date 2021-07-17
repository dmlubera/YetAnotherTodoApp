using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Api.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class UpdateStepTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid stepId, object request)
            => await TestClient.PutAsync($"api/todo/steps/{stepId}", GetContent(request));

        [Fact]
        public async Task WithValidData_ShouldReturnOkAndUpdateStepInDatabase()
        {
            var stepToUpdate = User.TodoLists.SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Title.Value == "TodoWithAssignedStep")
                .Steps.FirstOrDefault();
            var request = new UpdateStepRequest
            {
                Title = "UpdatedTitle",
                Description = "UpdatedDescription",
            };

            var httpResponse = await HandleRequestAsync(() => ActAsync(stepToUpdate.Id, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var todo = await DbContext.GetAsync<Step>(stepToUpdate.Id);
            todo.Title.Value.Should().Be(request.Title);
            todo.Description.Should().Be(request.Description);
        }

        [Fact]
        public async Task WhenStepDoesNotExist_ShouldReturnCustomErrorCode()
        {
            var stepId = Guid.NewGuid();
            var expectedException = new StepWithGivenIdDoesNotExistException(stepId);
            var request = new UpdateStepRequest
            {
                Title = "UpdatedTitle",
                Description = "UpdatedDescription",
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(stepId, request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task WithoutTitle_ShouldReturnValidationError()
        {
            var request = new
            {
                Description = "Description"
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(Guid.NewGuid(), request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task WithEmptyTitle_ShouldReturnValidationError()
        {
            var request = new UpdateStepRequest
            {
                Title = string.Empty,
                Description = "UpdatedDescription",
            };

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ValidationErrorResponse>(() => ActAsync(Guid.NewGuid(), request));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Errors.Should().NotBeEmpty();
        }
    }
}