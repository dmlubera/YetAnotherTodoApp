using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class AddTodoCommand : ICommand
    {
        public Guid CacheId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Project { get; set; }
        public DateTime FinishDate { get; set; }
        public List<StepDto> Steps { get; set; } = new List<StepDto>();

        public AddTodoCommand(Guid userId, string title, string project, DateTime finishDate, IEnumerable<StepDto> steps)
        {
            UserId = userId;
            Title = title;
            Project = project;
            FinishDate = finishDate;
            CacheId = Guid.NewGuid();
            if (steps is { })
                Steps.AddRange(steps);
        }
    }
}