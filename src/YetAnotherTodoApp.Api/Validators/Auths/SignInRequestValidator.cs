using FluentValidation;
using YetAnotherTodoApp.Api.Models.Auths;

namespace YetAnotherTodoApp.Api.Validators.Auths
{
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}