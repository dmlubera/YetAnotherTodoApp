using System;
using System.Text.RegularExpressions;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public class Name : IEquatable<Name>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        protected Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public bool Equals(Name other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstName == other.FirstName && LastName == other.LastName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Name) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName);
        }

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