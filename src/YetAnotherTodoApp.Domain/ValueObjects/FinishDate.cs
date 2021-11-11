using System;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public sealed class FinishDate : IEquatable<FinishDate>
    {
        public DateTime Value { get; private set; }

        private FinishDate() { }

        private FinishDate(DateTime value)
            => Value = value;

        public override bool Equals(object obj)
            => obj is FinishDate date && Value == date.Value;

        public bool Equals(FinishDate other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override int GetHashCode()
            => HashCode.Combine(Value);

        public static implicit operator DateTime(FinishDate finishDate)
            => finishDate.Value;

        public static implicit operator FinishDate(DateTime value)
            => new FinishDate(value);

        public static bool operator ==(FinishDate a, FinishDate b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is not null && b is not null)
                return a.Value.Equals(b.Value);

            return false;
        }

        public static bool operator !=(FinishDate a, FinishDate b)
            => !(a == b);

        public static FinishDate Create(DateTime date)
        {
            if (date.Date < DateTime.UtcNow.Date)
                throw new DateCannotBeEarlierThanTodayDateException(date.Date);

            return new FinishDate(date.Date);
        }
    }
}