namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InvalidUsernameException : DomainException
    {
        public override string Code => "invalid_username";

        public InvalidUsernameException(string username) : base($"Username {username} is invalid.")
        {
        }
    }
}
