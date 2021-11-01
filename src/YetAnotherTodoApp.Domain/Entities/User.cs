using System;
using System.Collections.Generic;
using System.Linq;
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
        public IReadOnlyCollection<TodoList> TodoLists => _todoLists.AsReadOnly();

        protected User()
        {
        }

        public User(string username, string email, string password, string salt)
        {
            Id = Guid.NewGuid();
            Username = Username.Create(username);
            Email = Email.Create(email);
            Password = Password.Create(password, salt);
            AddTodoList(new TodoList("Inbox"));
            UpdateAuditInfo();
        }

        public void AddTodoList(TodoList todoList)
        {
            if (_todoLists.Any(x => x.Title == todoList.Title))
                throw new TodoListWithGivenTitleAlreadyExistsException(todoList.Title);

            _todoLists.Add(todoList);
        }

        public void DeleteTodoList(Guid id)
        {
            var todoList = _todoLists.FirstOrDefault(x => x.Id == id);
            if (todoList is null)
                throw new TodoListWithGivenIdDoesNotExistException(id);
            if (todoList.Title == "Inbox")
                throw new InboxDeletionIsNotAllowedException();

           _todoLists.Remove(todoList);
        }

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