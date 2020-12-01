using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Queries.Models;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers
{
    public class GetTodosQueryHandler : IQueryHandler<GetTodosQuery, IEnumerable<Todo>>
    {
        private readonly IUserRepository _userRepository;

        public GetTodosQueryHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<IEnumerable<Todo>> HandleAsync(GetTodosQuery query)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId);
            return user.TodoLists.SelectMany(x => x.Todos).ToList();
        }
    }
}
