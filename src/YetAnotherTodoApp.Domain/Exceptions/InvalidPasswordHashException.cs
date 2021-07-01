namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidPasswordHashException : DomainException
    {
        public override string Code => "invalid_password_hash";

        public InvalidPasswordHashException()
            : base($"Given password's hash has invalid value.")
        {
        }
    }
}
