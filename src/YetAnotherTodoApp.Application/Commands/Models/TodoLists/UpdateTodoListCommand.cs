using System;

namespace YetAnotherTodoApp.Application.Commands.Models.TodoLists
{
    public class UpdateTodoListCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoListId { get; set; }
        public string Title { get; set; }

        private UpdateTodoListCommand() { }
        public UpdateTodoListCommand(Guid userId, Guid todoListId, string title)
        {
            UserId = userId;
            TodoListId = todoListId;
            Title = title;
        }
    }
}