using System.Collections.Generic;

namespace YetAnotherTodoApp.Api.Models.Errors
{
    public class ValidationErrorResponse
    {
        public IList<ValidationErrorModel> Errors { get; set; } = new List<ValidationErrorModel>();
    }
}