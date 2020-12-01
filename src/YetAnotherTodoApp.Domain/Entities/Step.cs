using System;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class Step : BaseEntity
    {
        public string Name { get; private set; }
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

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be empty.");
            Name = name;
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}