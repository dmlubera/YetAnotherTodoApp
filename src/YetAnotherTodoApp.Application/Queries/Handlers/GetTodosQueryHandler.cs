using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers
{
    public class GetTodosQueryHandler : IQueryHandler<GetTodosQuery, IEnumerable<TodoDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetTodosQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoDto>> HandleAsync(GetTodosQuery query)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId);
            return _mapper.Map<IEnumerable<TodoDto>>(user.TodoLists.SelectMany(x => x.Todos).ToList());
        }
    }
}
