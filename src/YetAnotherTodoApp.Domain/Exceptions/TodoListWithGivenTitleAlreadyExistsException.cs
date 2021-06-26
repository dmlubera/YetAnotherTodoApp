namespace YetAnotherTodoApp.Domain.Exceptions
{
    public class TodoListWithGivenTitleAlreadyExistsException : DomainException
    {
        public override string Code => "todo_list_with_given_title_already_exists";

        public TodoListWithGivenTitleAlreadyExistsException(string title)
            : base($"Todo list with title: {title} already exists.")
        {

        }
    }
}