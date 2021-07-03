using FluentValidation;
using YetAnotherTodoApp.Api.Models.Users;

namespace YetAnotherTodoApp.Api.Validators.Users
{
    public class UpdateEmailRequestValidator : AbstractValidator<UpdateEmailRequest>
    {
        public UpdateEmailRequestValidator()
            => RuleFor(x => x.Email).NotEmpty();
    }
}