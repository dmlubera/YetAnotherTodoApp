using FluentValidation;
using YetAnotherTodoApp.Api.Models.TodoLists;

namespace YetAnotherTodoApp.Api.Validators.TodoLists
{
    public class UpdateTodoListRequestValidator : AbstractValidator<UpdateTodoListRequest>
    {
        public UpdateTodoListRequestValidator()
            => RuleFor(x => x.Title).NotEmpty();
    }
}