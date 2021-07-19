using FluentValidation;
using YetAnotherTodoApp.Api.Models.Users;

namespace YetAnotherTodoApp.Api.Validators.Users
{
    public class UpdateUserInfoRequestValidator : AbstractValidator<UpdateUserInfoRequest>
    {
        public UpdateUserInfoRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }
}