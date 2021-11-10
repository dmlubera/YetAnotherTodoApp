using System;

namespace YetAnotherTodoApp.Application.Exceptions
{
    public class TodoTaskWithGivenIdDoesNotExistException : ApplicationException
    {
        public override string Code => "task_with_given_id_does_not_exist";

        public TodoTaskWithGivenIdDoesNotExistException(Guid id)
            : base($"Task with id: {id} does not exist.")
        {

        }
    }
}