using FluentValidation;
using YetAnotherTodoApp.Api.Models.TodoLists;

namespace YetAnotherTodoApp.Api.Validators.TodoLists
{
    public class CreateTodoListRequestValidator : AbstractValidator<CreateTodoListRequest>
    {
        public CreateTodoListRequestValidator()
            => RuleFor(x => x.Title).NotEmpty();
    }
}