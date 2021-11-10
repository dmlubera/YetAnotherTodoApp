using FluentValidation;
using YetAnotherTodoApp.Api.Models.TodoTasks;

namespace YetAnotherTodoApp.Api.Validators.TodoTasks
{
    public class UpdateTodoTaskRequestValidator : AbstractValidator<UpdateTodoTaskRequest>
    {
        public UpdateTodoTaskRequestValidator()
            => RuleFor(x => x.Title).NotEmpty();
    }
}