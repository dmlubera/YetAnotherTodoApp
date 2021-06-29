namespace YetAnotherTodoApp.Application.Exceptions
{
    public class UpdatePasswordToAlreadyUsedValueException : ApplicationException
    {
        public override string Code => "cannot_update_password_with_already_used_value";

        public UpdatePasswordToAlreadyUsedValueException() : base("Cannot update password with already used value.")
        {
        }
    }
}
