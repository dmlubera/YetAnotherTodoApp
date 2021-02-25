namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidPasswordHashException : DomainException
    {
        public override string Code => "invalid_password_hash";

        public InvalidPasswordHashException(string hash)
            : base($"Password hash: {hash} is invalid.")
        {
        }
    }
}
