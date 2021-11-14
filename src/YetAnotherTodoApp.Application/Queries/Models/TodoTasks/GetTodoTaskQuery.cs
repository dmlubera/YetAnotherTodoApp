using System;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Queries.Models.TodoTasks
{
    public class GetTodoTaskQuery : IQuery<TodoTaskDto>
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }

        public GetTodoTaskQuery(Guid userId, Guid taskId)
        {
            UserId = userId;
            TaskId = taskId;
        }
    }
}