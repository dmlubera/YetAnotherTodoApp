﻿namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidEmailFormatException : DomainException
    {
        public override string Code => "invalid_email_format";

        public InvalidEmailFormatException(string email)
            : base($"Given email: {email} has invalid format.")
        {
        }
    }
}
