namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidTitleException : DomainException
    {
        public override string Code => "invalid_title";

        public InvalidTitleException(string title)
            : base($"Given title: {title} has invalid value.")
        {
        }
    }
}