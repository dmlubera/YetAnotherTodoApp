namespace YetAnotherTodoApp.Application.Exceptions
{
    public class TodoListWithGivenTitleAlreadyExistsException : ApplicationException
    {
        public override string Code => "todolist_with_given_title_already_exists";

        public TodoListWithGivenTitleAlreadyExistsException(string title)
            : base($"Todo listi with title: {title} already exists.")
        {

        }
    }
}