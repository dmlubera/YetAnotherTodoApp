using System;
using YetAnotherTodoApp.Domain.Extensions;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }

        private User() { }

        public User(string username, string email, string password, string salt)
        {
            Id = Guid.NewGuid();
            SetUsername(username);
            SetEmail(email);
            SetPassword(password, salt);
            CreatedAt = DateTime.UtcNow;
        }

        public void SetUsername(string username)
        {
            username.IsEmpty();
            Username = username;
        }

        public void SetEmail(string email)
        {
            email.IsEmpty();
            Email = email;
        }
        
        public void SetPassword(string password, string salt)
        {
            password.IsEmpty();
            salt.IsEmpty();
            Password = password;
            Salt = salt;
        }
    }
}