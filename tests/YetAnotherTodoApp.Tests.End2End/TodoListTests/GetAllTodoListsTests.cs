using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Tests.End2End.TodoListTests
{
    public class GetAllTodoListsTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync()
            => await TestClient.GetAsync("/api/todolist/");

        [Fact]
        public async Task WithoutFilters_ReturnsHttpStatusCodeOkWithAllTodoLists()
        {
            await AuthenticateTestUserAsync();
            var response = await ActAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var resources = JsonConvert.DeserializeObject<List<TodoList>>(await response.Content.ReadAsStringAsync());
            resources.Count.Should().BeGreaterThan(0);
        }

        public async Task WithPagination_ReturnsHttpStatusCodeOkWithPaginatedTodoLists()
        {

        }
    }
}
