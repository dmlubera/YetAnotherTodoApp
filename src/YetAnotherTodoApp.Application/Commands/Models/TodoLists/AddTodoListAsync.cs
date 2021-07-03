using System;

namespace YetAnotherTodoApp.Application.Commands.Models.TodoLists
{
    public class AddTodoListAsync : ICommand
    {
        public Guid CacheTokenId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }

        public AddTodoListAsync(Guid id, string title)
        {
            UserId = id;
            Title = title;
            CacheTokenId = Guid.NewGuid();
        }
    }
}
