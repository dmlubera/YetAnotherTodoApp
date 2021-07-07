using System;

namespace YetAnotherTodoApp.Application.Commands.Models.TodoLists
{
    public class AddTodoListCommand : ICommand
    {
        public Guid CacheTokenId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }

        private AddTodoListCommand() { }

        public AddTodoListCommand(Guid id, string title)
        {
            UserId = id;
            Title = title;
            CacheTokenId = Guid.NewGuid();
        }
    }
}
