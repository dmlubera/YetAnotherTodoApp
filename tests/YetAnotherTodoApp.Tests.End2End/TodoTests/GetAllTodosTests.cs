using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Tests.End2End.TodoTests
{
    public class GetAllTodosTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync()
            => await TestClient.GetAsync("api/todo");

        [Fact]
        public async Task WithoutAnyFilters_ReturnsHttpStatusCodeOkWithAllTodos()
        {
            var expectedTodos = User.TodoLists.SelectMany(x => x.Todos).ToList();

            await AuthenticateTestUserAsync();
            var response = await ActAsync();
            var todos = JsonConvert.DeserializeObject<IList<TodoDto>>(await response.Content.ReadAsStringAsync());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            todos.Count.Should().Be(expectedTodos.Count);
        }

        public async Task WithPagination_ReturnsHttpStatusCodeOkWithPaginatedTodos()
        {

        }

        public async Task WithFilterBasedOnStatus_ReturnsHttpStatusCodeOkWithTodosWithSpecifiedStatus()
        {

        }

        public async Task WithFilterBasedOnTodoList_ReturnsHttpStatusCodeOkWithTodosAssignedToSpecifiedTodoList()
        {

        }

        public async Task WithSortingBasedOnPriority_ReturnsHttpStatusCodeOkWithTodosSortedByPriority()
        {

        }

        public async Task WithSortingBasedOnFinishDate_ReturnsHttpStatusCodeOkWithTodosSortedByFinishDate()
        {
        
        }
    }
}
