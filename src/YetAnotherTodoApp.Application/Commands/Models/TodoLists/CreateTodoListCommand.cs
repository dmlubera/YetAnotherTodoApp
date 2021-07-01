using System;

namespace YetAnotherTodoApp.Application.Commands.Models.TodoLists
{
    public class CreateTodoListCommand : ICommand
    {
        public Guid CacheTokenId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }

        public CreateTodoListCommand(Guid id, string title)
        {
            UserId = id;
            Title = title;
            CacheTokenId = Guid.NewGuid();
        }
    }
}
