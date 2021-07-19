using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries.Models.Todos;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers.Todos
{
    public class GetTodosQueryHandler : IQueryHandler<GetTodosQuery, IEnumerable<TodoDto>>
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;

        public GetTodosQueryHandler(ITodoRepository repository, IMapper mapper)
            => (_repository, _mapper) = (repository, mapper);

        public async Task<IEnumerable<TodoDto>> HandleAsync(GetTodosQuery query)
            => _mapper.Map<IEnumerable<TodoDto>>(await _repository.GetAllForUserAsync(query.UserId));
    }
}