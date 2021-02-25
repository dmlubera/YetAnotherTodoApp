namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidPasswordSaltException : DomainException
    {
        public override string Code => "invalid_password_salt";

        public InvalidPasswordSaltException(string salt)
            : base($"Password salt: {salt} is invalid.")
        {
        }
    }
}
