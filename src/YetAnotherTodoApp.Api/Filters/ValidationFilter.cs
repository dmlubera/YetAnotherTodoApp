using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Api.Models.Errors;

namespace YetAnotherTodoApp.Api.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToList()
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

                var errorResponse = new ValidationErrorResponse();

                foreach(var error in errorsInModelState)
                {
                    var errorModel = new ValidationErrorModel
                    {
                        Property = error.Key,
                        Messages = error.Value.ToList()
                    };
                    errorResponse.Errors.Add(errorModel);
                }

                var content = JsonConvert.SerializeObject(errorResponse);
                context.Result = new BadRequestObjectResult(content);

                return;
            }

            await next();
        }
    }
}