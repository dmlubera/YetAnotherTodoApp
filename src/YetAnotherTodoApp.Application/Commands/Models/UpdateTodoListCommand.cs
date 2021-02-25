using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdateTodoListCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoListId { get; set; }
        public string Title { get; set; }
    }
}