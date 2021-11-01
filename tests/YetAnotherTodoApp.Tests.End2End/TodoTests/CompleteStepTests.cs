using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class CompleteStepTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid stepId)
            => await TestClient.PutAsync($"api/todo/steps/{stepId}/complete", null);

        [Fact]
        public async Task WhenStepExist_ShouldMarkStepAsCompletedAndSaveChangesToDatabase()
        {
            var stepToComplete = User.TodoLists
                .SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Title == TestDbConsts.TestTodo)
                .Steps.FirstOrDefault();

            var httpResponse = await HandleRequestAsync(() => ActAsync(stepToComplete.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var step = await DbContext.GetAsync<Step>(stepToComplete.Id);
            step.IsFinished.Should().Be(true);
        }

        [Fact]
        public async Task WhenStepDoesNotExist_ShouldReturnCustomError()
        {
            var id = Guid.NewGuid();
            var expectedException = new StepWithGivenIdDoesNotExistException(id); 

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }

    }
}