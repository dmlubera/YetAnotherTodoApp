using System;

namespace YetAnotherTodoApp.Application.Exceptions
{
    public class TodoListWithGivenIdDoesNotExistException : ApplicationException
    {
        public override string Code => "todolist_with_given_id_does_not_exist";

        public TodoListWithGivenIdDoesNotExistException(Guid id)
            : base($"Todo list with id: {id} does not exist.")
        {

        }
    }
}