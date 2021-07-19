using System.Threading.Tasks;

namespace YetAnotherTodoApp.Application.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> HandleAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}