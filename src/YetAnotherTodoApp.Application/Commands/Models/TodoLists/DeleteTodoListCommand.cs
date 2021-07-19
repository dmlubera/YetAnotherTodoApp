using System;

namespace YetAnotherTodoApp.Application.Commands.Models.TodoLists
{
    public class DeleteTodoListCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoListId { get; set; }

        public DeleteTodoListCommand(Guid userId, Guid todoListId)
            => (UserId, TodoListId) = (userId, todoListId);
    }
}