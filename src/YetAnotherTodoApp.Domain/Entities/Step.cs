using System;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class Step : AuditableEntity
    {
        public Title Title { get; private set; }
        public string Description { get; private set; }
        public bool IsFinished { get; private set; }
        public Todo Todo { get; private set; }

        protected Step() { }

        public Step(string title, string description = null)
        {
            Id = Guid.NewGuid();
            Title = Title.Create(title);
            Description = description;
            UpdateAuditInfo();
        }

        public void Complete()
        {
            IsFinished = true;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Update(string title, string description)
        {
            Title = Title.Create(title);
            Description = description;
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}