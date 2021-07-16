using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.TodoLists;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.TodoLists
{
    public class UpdateTodoListRequstExample : IExamplesProvider<UpdateTodoListRequest>
    {
        public UpdateTodoListRequest GetExamples()
            => new UpdateTodoListRequest
            {
                Title = "Studies"
            };
    }
}