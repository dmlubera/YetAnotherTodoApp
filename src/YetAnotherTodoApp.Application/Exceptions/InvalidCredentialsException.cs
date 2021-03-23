namespace YetAnotherTodoApp.Application.Exceptions
{
    public class InvalidCredentialsException : ApplicationException
    {
        public override string Code => "invalid_credentials";

        public InvalidCredentialsException()
            : base("Given credentials are invalid.")
        {
        }
    }
}
