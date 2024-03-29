using System;
using System.Collections.Generic;
using System.Linq;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class Todo : BaseEntity
    {
        private readonly List<Step> _steps = new List<Step>();
        public Title Title { get; private set; }
        public string Description { get; private set; }
        public FinishDate FinishDate { get; private set; }
        public TodoStatus Status { get; private set; }
        public TodoPriority Priority { get; private set; }
        public TodoList TodoList { get; private set; }
        public IReadOnlyCollection<Step> Steps => _steps.AsReadOnly();

        protected Todo() { }

        public Todo(string title, DateTime finishDate, string description = null, TodoPriority priority = TodoPriority.Normal)
        {
            Id = Guid.NewGuid();
            Title = Title.Create(title);
            FinishDate = FinishDate.Create(finishDate);
            if (description is { })
                Description = description;
            Priority = priority;
            UpdateAuditInfo();
        }

        public void Update(string title, string descrtiption, DateTime finishDate)
        {
            Title = Title.Create(title);
            Description = descrtiption;
            FinishDate = FinishDate.Create(finishDate);
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(TodoStatus updatedStatus)
        {
            if (updatedStatus == TodoStatus.Done && _steps.Any(x => !x.IsFinished))
                throw new CannotChangeStatusToDoneOfTodoWithUnfinishedStepException();

            Status = updatedStatus;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdatePriority(TodoPriority updatedPriority)
        {
            Priority = updatedPriority;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void AddSteps(IEnumerable<Step> steps)
            => _steps.AddRange(steps);

        public void RemoveStep(Guid stepId)
        {
            var step = _steps.FirstOrDefault(x => x.Id == stepId);
            _steps.Remove(step);
        }
    }
}