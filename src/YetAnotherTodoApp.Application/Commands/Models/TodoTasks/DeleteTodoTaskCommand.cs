using System;

namespace YetAnotherTodoApp.Application.Commands.Models.TodoTasks
{
    public class DeleteTodoTaskCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public Guid TaskId { get; set; }

        protected DeleteTodoTaskCommand() { }

        public DeleteTodoTaskCommand(Guid userId, Guid todoId, Guid taskId)
        {
            UserId = userId;
            TodoId = todoId;
            TaskId = taskId;
        }
    }
}