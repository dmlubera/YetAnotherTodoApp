using System;

namespace YetAnotherTodoApp.Application.Commands.Models.Steps
{
    public class DeleteStepCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public Guid StepId { get; set; }

        protected DeleteStepCommand() { }

        public DeleteStepCommand(Guid userId, Guid todoId, Guid stepId)
        {
            UserId = userId;
            TodoId = todoId;
            StepId = stepId;
        }
    }
}