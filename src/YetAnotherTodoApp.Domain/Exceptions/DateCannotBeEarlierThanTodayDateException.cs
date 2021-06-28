using System;

namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class DateCannotBeEarlierThanTodayDateException : DomainException
    {
        public override string Code => "date_cannot_be_earlier_that_today_date";

        public DateCannotBeEarlierThanTodayDateException(DateTime date)
            : base($"Date: {date} is earlier than today date and cannot be set as todo's finish date.")
        {

        }
    }
}