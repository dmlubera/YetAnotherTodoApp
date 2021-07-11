using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Todos
{
    public class DeleteTodoCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }

        protected DeleteTodoCommand() { }
        public DeleteTodoCommand(Guid userId, Guid todoId)
            => (UserId, TodoId) = (userId, todoId);
    }
}