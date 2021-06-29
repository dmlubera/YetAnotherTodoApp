namespace YetAnotherTodoApp.Application.Exceptions
{
    public class UpdateEmailToAlreadyUsedValueException : ApplicationException
    {
        public override string Code => "cannot_update_email_with_already_used_value";

        public UpdateEmailToAlreadyUsedValueException() : base("Cannot update password with already used value.")
        {
        }
    }
}
