using System;

namespace YetAnotherTodoApp.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime LastModifiedAt { get; protected set; }

        protected void UpdateAuditInfo()
        {
            var now = DateTime.UtcNow;
            CreatedAt = now;
            LastModifiedAt = now;
        }
    }
}