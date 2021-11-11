using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Application.Commands.Models.Todos
{
    public class AddTodoCommand : ICommand
    {
        public Guid CacheTokenId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TodoPriority? Priority { get; set; }
        public string TodoList { get; set; }
        public DateTime FinishDate { get; set; }
        public List<TodoTaskDto> Tasks { get; set; } = new List<TodoTaskDto>();

        protected AddTodoCommand() { }

        public AddTodoCommand(Guid userId, string title, string todoList, DateTime finishDate,
            string description, TodoPriority? priority, IEnumerable<TodoTaskDto> tasks)
        {
            UserId = userId;
            Title = title;
            Description = description;
            Priority = priority;
            TodoList = todoList;
            FinishDate = finishDate;
            CacheTokenId = Guid.NewGuid();
            if (tasks is { })
                Tasks.AddRange(tasks);
        }
    }
}