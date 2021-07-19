namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class InboxDeletionIsNotAllowedException : DomainException
    {
        public override string Code => "inbox_deletion_not_allowed";

        public InboxDeletionIsNotAllowedException()
            : base("Inbox deletion is not allowed.")
        {

        }
    }
}