namespace YetAnotherTodoApp.Application.Exceptions
{
    public class InboxModificationIsNotAllowedException : ApplicationException
    {
        public override string Code => "inbox_modification_not_allowed";

        public InboxModificationIsNotAllowedException()
            : base("Inbox modification is not allowed.")
        {

        }
    }
}