﻿using System;
using System.Text.RegularExpressions;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public class Username : IEquatable<Username>
    {
        public string Value { get; private set; }

        protected Username() { }

        protected Username(string value)
            => Value = value;

        public bool Equals(Username other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Username) obj);
        }

        public override int GetHashCode() 
            => Value != null ? Value.GetHashCode() : 0;

        public static implicit operator string(Username username)
            => username.Value;
        
        public static implicit operator Username(string value)
            => new Username(value);

        public static bool operator ==(Username a, Username b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is not null && b is not null)
                return a.Value.Equals(b.Value);

            return false;
        }

        public static bool operator !=(Username a, Username b)
            => !(a == b);

        public static Username Create(string username)
        {
            var regexPattern = @"^\w[a-zA-Z0-9_]+\w{5,}$";
            if (!Regex.IsMatch(username, regexPattern))
                throw new InvalidUsernameException(username);

            return new Username(username);
        }
    }
}