using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class AddTodoCommand : ICommand
    {
        public Guid CacheId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Project { get; set; }
        public DateTime FinishDate { get; set; }

        public AddTodoCommand(Guid userId, string title, string project, DateTime finishDate)
        {
            UserId = userId;
            Title = title;
            Project = project;
            FinishDate = finishDate;
            CacheId = Guid.NewGuid();
        }
    }
}