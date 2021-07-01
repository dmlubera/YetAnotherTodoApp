using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Todos
{
    public class UpdateTodoCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime FinishDate { get; set; }

        public UpdateTodoCommand(Guid userId, Guid todoId, string title, string description, DateTime finishDate)
        {
            UserId = userId;
            TodoId = todoId;
            Title = title;
            Description = description;
            FinishDate = finishDate;
        }
    }
}