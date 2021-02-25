using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class DeleteTodoListCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoListId { get; set; }
    }
}