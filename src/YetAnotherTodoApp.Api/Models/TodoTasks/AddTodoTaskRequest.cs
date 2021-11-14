using System;

namespace YetAnotherTodoApp.Api.Models.TodoTasks
{
    public class AddTodoTaskRequest
    {
        public Guid TodoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}