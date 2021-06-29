using System;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.ValueObjects
{
    public class FinishDate : IEquatable<FinishDate>
    {
        public DateTime Value { get; private set; }

        protected FinishDate() { }

        protected FinishDate(DateTime value)
            => Value = value;

        public override bool Equals(object obj)
            => obj is FinishDate date && Value == date.Value;

        public bool Equals(FinishDate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value && Value == other.Value;
        }

        public override int GetHashCode()
            => HashCode.Combine(Value);

        public static FinishDate Create(DateTime date)
        {
            if (date.Date < DateTime.UtcNow.Date)
                throw new DateCannotBeEarlierThanTodayDateException(date.Date);

            return new FinishDate(date.Date);
        }

    }
}