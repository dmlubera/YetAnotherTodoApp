namespace YetAnotherTodoApp.Application.Exceptions
{
    public class UsernameInUseException : ApplicationException
    {
        public override string Code => "username_in_use";

        public UsernameInUseException(string username)
            : base($"Username: {username} is in use.")
        {
        }
    }
}