namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidLastNameException : DomainException
    {
        public override string Code => "invalid_lastname";

        public InvalidLastNameException(string lastName)
            : base($"Lastname: {lastName} is invalid.")
        {
        }
    }
}
