using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Steps
{
    public class UpdateStepCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid StepId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        protected UpdateStepCommand() { }

        public UpdateStepCommand(Guid userId, Guid stepId, string title, string description)
        {
            UserId = userId;
            StepId = stepId;
            Title = title;
            Description = description;
        }
    }
}