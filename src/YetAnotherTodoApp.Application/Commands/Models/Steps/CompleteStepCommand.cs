using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Steps
{
    public class CompleteStepCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid StepId { get; set; }

        protected CompleteStepCommand() { }

        public CompleteStepCommand(Guid userId, Guid stepId)
            => (UserId, StepId) = (userId, stepId);
    }
}