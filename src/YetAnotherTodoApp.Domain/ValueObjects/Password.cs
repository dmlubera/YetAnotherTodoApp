using System;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public class Password : IEquatable<Password>
    {
        public string Hash { get; private set; }
        public string Salt { get; private set; }

        public Password(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }

        public bool Equals(Password other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Hash == other.Hash && Salt == other.Salt;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Password) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hash, Salt);
        }
    }
}