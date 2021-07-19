using FluentValidation;
using YetAnotherTodoApp.Api.Models.Todos;

namespace YetAnotherTodoApp.Api.Validators.Todos
{
    public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
    {
        public UpdateTodoRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.FinishDate).NotEmpty();
        }
    }
}