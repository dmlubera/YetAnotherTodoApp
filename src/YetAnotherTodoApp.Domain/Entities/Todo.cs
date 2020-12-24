using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using YetAnotherTodoApp.Domain.Enums;
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
        [JsonIgnore]
        public virtual TodoList TodoList { get; private set; }
        [JsonIgnore]
        public virtual IReadOnlyCollection<Step> Steps => _steps.AsReadOnly();

        protected Todo() { }

        public Todo(string title, DateTime finishDate)
        {
            Id = Guid.NewGuid();
            SetTitle(title);
            SetFinishDate(finishDate);
            SetPriority(TodoPriority.Normal);
            CreatedAt = DateTime.UtcNow;
        }

        public void SetTitle(string title)
        {
            Title = Title.Create(title);
            LastModifiedAt = DateTime.UtcNow;
        }

        public void SetFinishDate(DateTime finishDate)
        {
            if (finishDate == null)
                throw new ArgumentException("Value cannot be empty.");

            FinishDate = finishDate;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void SetPriority(TodoPriority priority)
        {
            Priority = priority;
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}