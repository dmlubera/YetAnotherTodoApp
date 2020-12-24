using System;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class Step : BaseEntity
    {
        public virtual Title Title { get; private set; }
        public string Description { get; private set; }
        public bool IsFinished { get; private set; }
        public virtual Todo Todo { get; private set; }

        protected Step() { }

        public Step(string name)
        {
            Id = Guid.NewGuid();
            SetName(name);
            CreatedAt = DateTime.UtcNow;
        }

        public void SetName(string title)
        {
            Title = Title.Create(title);
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}