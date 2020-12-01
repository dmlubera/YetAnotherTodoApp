using System;

namespace YetAnotherTodoApp.Api.Models
{
    public class AddTodoRequest
    {
        public string Title { get; set; }
        public string Project { get; set; }
        public DateTime FinishDate { get; set; }
    }
}