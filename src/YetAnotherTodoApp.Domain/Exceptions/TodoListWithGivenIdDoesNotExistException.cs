using System;

namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class TodoListWithGivenIdDoesNotExistException : DomainException
    {
        public override string Code => "todolist_with_given_id_does_not_exist";

        public TodoListWithGivenIdDoesNotExistException(Guid id)
            : base($"Todo list with id: {id} does not exist.")
        {

        }
    }
}