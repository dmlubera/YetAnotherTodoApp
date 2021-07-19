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
        public string Project { get; set; }
        public DateTime FinishDate { get; set; }
        public List<StepDto> Steps { get; set; } = new List<StepDto>();

        protected AddTodoCommand() { }

        public AddTodoCommand(Guid userId, string title, string project, DateTime finishDate,
            string description, TodoPriority? priority, IEnumerable<StepDto> steps)
        {
            UserId = userId;
            Title = title;
            Description = description;
            Priority = priority;
            Project = project;
            FinishDate = finishDate;
            CacheTokenId = Guid.NewGuid();
            if (steps is { })
                Steps.AddRange(steps);
        }
    }
}