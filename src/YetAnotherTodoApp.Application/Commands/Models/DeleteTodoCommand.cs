using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class DeleteTodoCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
    }
}
