using System;

namespace YetAnotherTodoApp.Application.Commands.Models.TodoTasks
{
    public class UpdateTodoTaskCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        protected UpdateTodoTaskCommand() { }

        public UpdateTodoTaskCommand(Guid userId, Guid taskId, string title, string description)
        {
            UserId = userId;
            TaskId = taskId;
            Title = title;
            Description = description;
        }
    }
}