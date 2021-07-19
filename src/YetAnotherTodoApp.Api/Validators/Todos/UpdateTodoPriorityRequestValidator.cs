using FluentValidation;
using YetAnotherTodoApp.Api.Models.Todos;

namespace YetAnotherTodoApp.Api.Validators.Todos
{
    public class UpdateTodoPriorityRequestValidator : AbstractValidator<UpdateTodoPriorityRequest>
    {
        public UpdateTodoPriorityRequestValidator()
            => RuleFor(x => x.Priority).IsInEnum();
    }
}