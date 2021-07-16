using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.TodoLists;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.TodoLists
{
    public class AddTodoListRequestExample : IExamplesProvider<AddTodoListRequest>
    {
        public AddTodoListRequest GetExamples()
            => new AddTodoListRequest
            {
                Title = "Work stuff"
            };
    }
}