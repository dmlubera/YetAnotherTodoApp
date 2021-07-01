using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries.Models.TodoLists;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers.TodoLists
{
    public class GetTodoListsQueryHandler : IQueryHandler<GetTodoListsQuery, IEnumerable<TodoListDto>>
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IMapper _mapper;

        public GetTodoListsQueryHandler(ITodoListRepository todoListRepository, IMapper mapper)
        {
            _todoListRepository = todoListRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoListDto>> HandleAsync(GetTodoListsQuery query)
            => _mapper.Map<IEnumerable<TodoListDto>>(await _todoListRepository.GetAllForUserAsync(query.UserId));
    }
}