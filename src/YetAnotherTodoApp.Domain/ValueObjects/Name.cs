using System;
using System.Text.RegularExpressions;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public class Name : IEquatable<Name>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        protected Name() { }

        protected Name(string firstName, string lastName)
            => (FirstName, LastName) = (firstName, lastName);

        public bool Equals(Name other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstName == other.FirstName && LastName == other.LastName;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Name) obj);
        }

        public override int GetHashCode() 
            => HashCode.Combine(FirstName, LastName);

        public static Name Create(string firstName, string lastName)
        {
            var regexPattern = @"^[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(firstName, regexPattern))
                throw new InvalidFirstNameException(firstName);
            if (!Regex.IsMatch(lastName, regexPattern))
                throw new InvalidLastNameException(lastName);

            return new Name(firstName, lastName);
        }
    }
}