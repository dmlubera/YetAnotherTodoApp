using System.Collections.Generic;

namespace YetAnotherTodoApp.Api.Models.Errors
{
    public class ValidationErrorModel
    {
        public string Property { get; set; }
        public IList<string> Messages { get; set; } = new List<string>();
    }
}