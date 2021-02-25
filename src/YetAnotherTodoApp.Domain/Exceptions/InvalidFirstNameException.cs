namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidFirstNameException : DomainException
    {
        public override string Code => "invalid_firstname";

        public InvalidFirstNameException(string firstName)
            : base($"Firstname: {firstName} is invalid.")
        {
        }
    }
}
