using System;

namespace YetAnotherTodoApp.Application.Exceptions
{
    public class UserNotExistException : ApplicationException
    {
        public override string Code => "user_not_exist";

        public UserNotExistException(Guid id)
            : base($"User with ID: {id} does not exist.")
        {
        }
    }
}