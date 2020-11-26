using System;
using System.Collections.Generic;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class TodoList : BaseEntity
    {
        private readonly List<Todo> _todos = new List<Todo>();
        public string Name { get; private set; }
        public virtual IReadOnlyCollection<Todo> Todos => _todos.AsReadOnly();

        private TodoList() { }

        public TodoList(string name)
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