using System;

namespace YetAnotherTodoApp.Application.Exceptions
{
    public class TodoWithGivenIdDoesNotExistException : ApplicationException
    {
        public override string Code => "todo_with_given_id_does_not_exist";

        public TodoWithGivenIdDoesNotExistException(Guid id)
            : base($"Todo with id: {id} does not exist.")
        {
            
        }
    }
}