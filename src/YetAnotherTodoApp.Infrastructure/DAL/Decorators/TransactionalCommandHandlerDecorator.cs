using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands;
using YetAnotherTodoApp.Infrastructure.DAL.UnitOfWork;

namespace YetAnotherTodoApp.Infrastructure.DAL.Decorators
{
    public class TransactionalCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
    {
        private readonly ICommandHandler<T> _handler;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionalCommandHandlerDecorator(ICommandHandler<T> handler, IUnitOfWork unitOfWork)
        {
            _handler = handler;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(T command)
        {
            await _unitOfWork.ExecuteAsync(() => _handler.HandleAsync(command));
        }
    }
}