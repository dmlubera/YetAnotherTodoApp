namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidFirstNameException : DomainException
    {
        public override string Code => "invalid_firstname";

        public InvalidFirstNameException(string firstName)
            : base($"Given first name: {firstName} has invalid value.")
        {
        }
    }
}
