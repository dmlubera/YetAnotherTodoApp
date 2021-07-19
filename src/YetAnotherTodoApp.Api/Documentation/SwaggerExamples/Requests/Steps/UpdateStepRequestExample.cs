using Swashbuckle.AspNetCore.Filters;
using YetAnotherTodoApp.Api.Models.Steps;

namespace YetAnotherTodoApp.Api.Documentation.SwaggerExamples.Requests.Steps
{
    public class UpdateStepRequestExample : IExamplesProvider<UpdateStepRequest>
    {
        public UpdateStepRequest GetExamples()
            => new UpdateStepRequest
            {
                Title = "Step of todo",
                Description = "First part of todo"
            };
    }
}