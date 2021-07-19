using System;

namespace YetAnotherTodoApp.Application.Exceptions
{
    public class StepWithGivenIdDoesNotExistException : ApplicationException
    {
        public override string Code => "step_with_given_id_does_not_exist";

        public StepWithGivenIdDoesNotExistException(Guid id)
            : base($"Step with id: {id} does not exist.")
        {

        }
    }
}