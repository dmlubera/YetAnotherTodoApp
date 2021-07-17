using FluentValidation;
using YetAnotherTodoApp.Api.Models.Steps;

namespace YetAnotherTodoApp.Api.Validators.Steps
{
    public class UpdateStepRequestValidator : AbstractValidator<UpdateStepRequest>
    {
        public UpdateStepRequestValidator()
            => RuleFor(x => x.Title).NotEmpty();
    }
}