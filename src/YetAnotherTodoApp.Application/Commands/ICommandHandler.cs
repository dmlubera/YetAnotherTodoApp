using System.Threading.Tasks;

namespace YetAnotherTodoApp.Application.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}