using FluentValidation;
using YetAnotherTodoApp.Api.Models.Auths;

namespace YetAnotherTodoApp.Api.Validators.Auths
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Username).NotEmpty().MinimumLength(6);
        }
    }
}