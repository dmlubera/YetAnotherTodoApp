using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class User : BaseEntity
    {
        private readonly List<TodoList> _todoLists = new List<TodoList>();
        public Username Username { get; private set; }
        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public virtual IReadOnlyCollection<TodoList> TodoLists => _todoLists.AsReadOnly();

        protected User() { }

        public User(string username, string email, string password, string salt)
        {
            Id = Guid.NewGuid();
            Username = new Username(username);
            Email = new Email(email);
            Password = new Password(password, salt);
            AddTodoList(new TodoList("Inbox"));
            CreatedAt = DateTime.UtcNow;
        }

        public void AddTodoList(TodoList todoList)
            => _todoLists.Add(todoList);
    }
}