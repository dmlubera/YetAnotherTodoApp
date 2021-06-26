using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class CreateTodoListCommand : ICommand
    {
        public Guid CacheToken { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }

        public CreateTodoListCommand(Guid id, string title)
        {
            UserId = id;
            Title = title;
            CacheToken = Guid.NewGuid();
        }
    }
}
