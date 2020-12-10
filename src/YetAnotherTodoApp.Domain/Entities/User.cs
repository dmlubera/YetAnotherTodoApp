using System;
using System.Collections.Generic;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class User : BaseEntity
    {
        private readonly List<TodoList> _todoLists = new List<TodoList>();
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }
        public virtual IReadOnlyCollection<TodoList> TodoLists => _todoLists.AsReadOnly();

        protected User() { }

        public User(string username, string email, string password, string salt)
        {
            Id = Guid.NewGuid();
            SetUsername(username);
            SetEmail(email);
            SetPassword(password, salt);
            AddTodoList(new TodoList("Inbox"));
            CreatedAt = DateTime.UtcNow;
        }

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidUsernameException(username);
            Username = username;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidEmailFormatException(email);
            Email = email;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SetPassword(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidPasswordException(password);

            Password = password;
            Salt = salt;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void AddTodoList(TodoList todoList)
            => _todoLists.Add(todoList);
    }
}