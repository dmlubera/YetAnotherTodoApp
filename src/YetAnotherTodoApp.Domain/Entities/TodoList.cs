using System;
using System.Collections.Generic;
using System.Linq;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class TodoList : BaseEntity
    {
        private readonly List<Todo> _todos = new List<Todo>();
        public Title Title { get; private set; }
        public User User { get; private set; }
        public IReadOnlyCollection<Todo> Todos => _todos.AsReadOnly();

        protected TodoList() { }

        public TodoList(string title)
        {
            Id = Guid.NewGuid();
            Title = Title.Create(title);
            UpdateAuditInfo();
        }

        public void AddTodo(Todo todo)
            => _todos.Add(todo);

        public void DeleteTodo(Guid todoId)
        {
            var todo = _todos.FirstOrDefault(x => x.Id == todoId);
            _todos.Remove(todo);
        }

        public void UpdateTitle(string title)
        {
            Title = Title.Create(title);
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}