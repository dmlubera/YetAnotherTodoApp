using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Tests.End2End.TodoListTests
{
    public class GetAllTodoListsTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync()
            => await TestClient.GetAsync("/api/todolist/");

        [Fact]
        public async Task WithoutFilters_ShouldReturnOkWithAllTodoLists()
        {
            (var httpResponse, var todoLists) =
                await HandleRequestAsync<IList<TodoListDto>>(() => ActAsync());

            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            todoLists.Count.Should().BeGreaterThan(0);
        }

        public async Task WithPagination_ReturnsHttpStatusCodeOkWithPaginatedTodoLists()
        {

        }
    }
}