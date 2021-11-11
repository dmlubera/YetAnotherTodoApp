using System;
using System.Text.RegularExpressions;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        public string Value { get; private set; }

        private Email() { }

        private Email(string value)
            => Value = value;

        public bool Equals(Email other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Email) obj);
        }

        public override int GetHashCode()
            => Value != null ? Value.GetHashCode() : 0;

        public static implicit operator string(Email email) => email.Value;
        
        public static implicit operator Email(string value) => new Email(value);

        public static bool operator ==(Email a, Email b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is not null && b is not null)
                return a.Value.Equals(b.Value);

            return false;
        }

        public static bool operator !=(Email a, Email b)
            => !(a == b);

        public static Email Create(string email)
        {
            var regexPattern = @"^(\w[a-zA-Z0-9]|(\w[a-zA-Z0-9._][a-zA-Z0-9]))+@[a-zA-Z0-9.-]+\w+\.[a-zA-Z0-9]{2,6}$";
            if (!Regex.IsMatch(email, regexPattern))
                throw new InvalidEmailFormatException(email);

            return new Email(email);
        }
    }
}