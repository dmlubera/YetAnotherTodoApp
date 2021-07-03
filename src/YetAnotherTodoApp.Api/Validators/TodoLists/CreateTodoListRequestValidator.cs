using FluentValidation;
using YetAnotherTodoApp.Api.Models.TodoLists;

namespace YetAnotherTodoApp.Api.Validators.TodoLists
{
    public class CreateTodoListRequestValidator : AbstractValidator<AddTodoListRequest>
    {
        public CreateTodoListRequestValidator()
            => RuleFor(x => x.Title).NotEmpty();
    }
}