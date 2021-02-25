using System;
using System.Text.RegularExpressions;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public class Username : IEquatable<Username>
    {
        public string Value { get; private set; }

        protected Username(string value)
            => Value = value;

        public bool Equals(Username other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Username) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static Username Create(string username)
        {
            var regexPattern = @"^\w[a-zA-Z0-9_]+\w{5,}$";
            if (!Regex.IsMatch(username, regexPattern))
                throw new InvalidUsernameException(username);

            return new Username(username);
        }
    }
}