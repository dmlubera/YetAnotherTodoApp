using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class Todo : BaseEntity
    {
        private readonly List<Step> _steps = new List<Step>();
        public virtual Title Title { get; private set; }
        public string Description { get; private set; }
        public DateTime FinishDate { get; private set; }
        public TodoStatus Status { get; private set; }
        public TodoPriority Priority { get; private set; }
        public virtual TodoList TodoList { get; private set; }
        public virtual IReadOnlyCollection<Step> Steps => _steps.AsReadOnly();

        protected Todo() { }

        public Todo(string title, DateTime finishDate)
        {
            Id = Guid.NewGuid();
            Title = Title.Create(title);
            SetFinishDate(finishDate);
            Priority = TodoPriority.Normal;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string title, string descrtiption, DateTime finishDate)
        {
            Title = Title.Create(title);
            Description = descrtiption;
            SetFinishDate(finishDate);
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(TodoStatus updatedStatus)
        {
            Status = updatedStatus;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdatePriority(TodoPriority updatedPriority)
        {
            Priority = updatedPriority;
            LastModifiedAt = DateTime.UtcNow;
        }

        private void SetFinishDate(DateTime date)
        {
            if (DateTime.UtcNow.Date > date.Date)
                throw new DateCannotBeEarlierThanTodayDateException(date.Date);

            FinishDate = date.Date;
        }
    }
}