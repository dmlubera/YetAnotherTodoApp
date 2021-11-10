namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class CannotChangeStatusToDoneOfTodoWithUnfinishedTaskException : DomainException
    {
        public override string Code => "cannot_done_todo_with_unfinished_task";

        public CannotChangeStatusToDoneOfTodoWithUnfinishedTaskException()
            : base("The status of todo with unifinished task cannot be changed to done.")
        {

        }
    }
}