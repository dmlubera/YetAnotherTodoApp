using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class TodoList : BaseEntity
    {
        private readonly List<Todo> _todos = new List<Todo>();
        public virtual Title Title { get; private set; }
        public virtual User User { get; private set; }
        public virtual IReadOnlyCollection<Todo> Todos => _todos.AsReadOnly();

        protected TodoList() { }

        public TodoList(string title)
        {
            Id = Guid.NewGuid();
            Title = Title.Create(title);
            CreatedAt = DateTime.UtcNow;
        }

        public void AddTodo(Todo todo)
            => _todos.Add(todo);
    }
}