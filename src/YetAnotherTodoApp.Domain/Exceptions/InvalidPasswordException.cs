namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidPasswordException : DomainException
    {
        public override string Code => "invalid_password";

        public InvalidPasswordException(string password) : base($"Password is invalid.")
        {
        }
    }
}
