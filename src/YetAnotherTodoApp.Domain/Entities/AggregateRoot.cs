using System;

namespace YetAnotherTodoApp.Domain.Entities
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
    }
}