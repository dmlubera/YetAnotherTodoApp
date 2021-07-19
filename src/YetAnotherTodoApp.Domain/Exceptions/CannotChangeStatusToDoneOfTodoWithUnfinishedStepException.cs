namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class CannotChangeStatusToDoneOfTodoWithUnfinishedStepException : DomainException
    {
        public override string Code => "cannot_done_todo_with_unfinished_step";

        public CannotChangeStatusToDoneOfTodoWithUnfinishedStepException()
            : base("The status of todo with unifinished step cannot be changed to done.")
        {

        }
    }
}