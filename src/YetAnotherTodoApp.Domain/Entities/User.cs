using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class User : BaseEntity
    {
        private readonly List<TodoList> _todoLists = new List<TodoList>();
        public virtual Username Username { get; private set; }
        public virtual Name Name { get; private set; }
        public virtual Email Email { get; private set; }
        public virtual Password Password { get; private set; }
        public virtual IReadOnlyCollection<TodoList> TodoLists => _todoLists.AsReadOnly();

        protected User() { }

        public User(string username, string email, string password, string salt)
        {
            Id = Guid.NewGuid();
            Username = Username.Create(username);
            Email = Email.Create(email);
            Password = Password.Create(password, salt);
            AddTodoList(new TodoList("Inbox"));
            CreatedAt = DateTime.UtcNow;
        }

        public void AddTodoList(TodoList todoList)
            => _todoLists.Add(todoList);

        public void RemoveTodoList(TodoList todoList)
            => _todoLists.Remove(todoList);

        public void UpdateUserInfo(string firstName, string lastname)
        {
            Name = Name.Create(firstName, lastname);
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(string email)
        {
            Email = Email.Create(email);
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdatePassword(string passwordHash, string passwordSalt)
        {
            Password = Password.Create(passwordHash, passwordSalt);
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}