using FluentValidation;
using YetAnotherTodoApp.Api.Models.Users;

namespace YetAnotherTodoApp.Api.Validators.Users
{
    public class UpdatePasswordRequestValidator : AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordRequestValidator() 
            => RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}