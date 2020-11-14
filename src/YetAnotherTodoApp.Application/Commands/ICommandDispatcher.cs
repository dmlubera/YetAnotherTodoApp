using System.Threading.Tasks;

namespace YetAnotherTodoApp.Application.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}