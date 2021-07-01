namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidPasswordSaltException : DomainException
    {
        public override string Code => "invalid_password_salt";

        public InvalidPasswordSaltException()
            : base($"Given password's salt has invalid value.")
        {
        }
    }
}
