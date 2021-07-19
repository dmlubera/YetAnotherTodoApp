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
        private readonly ITodoListRepository _repository;
        private readonly IMapper _mapper;

        public GetTodoListsQueryHandler(ITodoListRepository repository, IMapper mapper)
            => (_repository, _mapper) = (repository, mapper);

        public async Task<IEnumerable<TodoListDto>> HandleAsync(GetTodoListsQuery query)
            => _mapper.Map<IEnumerable<TodoListDto>>(await _repository.GetAllForUserAsync(query.UserId));
    }
}