namespace YetAnotherTodoApp.Application.Exceptions
{
    public class InvalidPasswordFormatException : ApplicationException
    {
        public override string Code => "invalid_password_format";

        public InvalidPasswordFormatException()
            : base("Given password has invalid format.")
        {
        }
    }
}