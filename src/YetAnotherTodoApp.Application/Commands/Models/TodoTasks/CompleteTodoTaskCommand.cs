using System;

namespace YetAnotherTodoApp.Application.Commands.Models.TodoTasks
{
    public class CompleteTodoTaskCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }

        protected CompleteTodoTaskCommand() { }

        public CompleteTodoTaskCommand(Guid userId, Guid taskId)
            => (UserId, TaskId) = (userId, taskId);
    }
}