using FluentValidation;
using YetAnotherTodoApp.Api.Models.Todos;

namespace YetAnotherTodoApp.Api.Validators.Todos
{
    public class AddTodoRequestValidator : AbstractValidator<AddTodoRequest>
    {
        public AddTodoRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.FinishDate).NotEmpty();
        }
    }
}