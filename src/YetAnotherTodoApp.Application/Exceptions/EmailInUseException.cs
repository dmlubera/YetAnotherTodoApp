namespace YetAnotherTodoApp.Application.Exceptions
{
    public class EmailInUseException : ApplicationException
    {
        public override string Code => "email_in_use";

        public EmailInUseException(string email)
            : base($"Email: {email} is in use.")
        {
        }
    }
}
