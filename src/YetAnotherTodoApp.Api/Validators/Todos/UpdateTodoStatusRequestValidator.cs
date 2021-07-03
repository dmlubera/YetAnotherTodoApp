using FluentValidation;
using YetAnotherTodoApp.Api.Models.Todos;

namespace YetAnotherTodoApp.Api.Validators.Todos
{
    public class UpdateTodoStatusRequestValidator : AbstractValidator<UpdateTodoStatusRequest>
    {
        public UpdateTodoStatusRequestValidator() 
            => RuleFor(x => x.Status).IsInEnum();
    }
}