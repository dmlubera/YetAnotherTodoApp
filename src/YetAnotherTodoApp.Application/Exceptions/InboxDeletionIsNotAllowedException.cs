namespace YetAnotherTodoApp.Application.Exceptions
{
    public class InboxDeletionIsNotAllowedException : ApplicationException
    {
        public override string Code => "inbox_deletion_not_allowed";

        public InboxDeletionIsNotAllowedException()
            : base("Inbox deletion is not allowed.")
        {

        }
    }
}