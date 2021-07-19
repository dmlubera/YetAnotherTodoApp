using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Steps
{
    public class AddStepCommand : ICommand
    {
        public Guid CacheTokenId { get; set; }
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        protected AddStepCommand() { }

        public AddStepCommand(Guid userId, Guid todoId, string title, string description = null)
        {
            CacheTokenId = Guid.NewGuid();
            UserId = userId;
            TodoId = todoId;
            Title = title;
            Description = description;
        }
    }
}