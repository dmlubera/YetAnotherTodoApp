using System;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public class Title : IEquatable<Title>
    {
        public string Value { get; private set; }

        protected Title(string value)
            => Value = value;

        public bool Equals(Title other)
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
            return Equals((Title) obj);
        }

        public override int GetHashCode()
            => Value != null ? Value.GetHashCode() : 0;

        public static Title Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidTitleException(title);

            return new Title(title);
        }
    }
}