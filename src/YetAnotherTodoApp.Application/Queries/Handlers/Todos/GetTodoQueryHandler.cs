using System.Threading.Tasks;
using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Queries.Models.Todos;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers.Todos
{
    public class GetTodoQueryHandler : IQueryHandler<GetTodoQuery, DetailedTodoDto>
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;

        public GetTodoQueryHandler(ITodoRepository repository, IMapper mapper)
            => (_repository, _mapper) = (repository, mapper);

        public async Task<DetailedTodoDto> HandleAsync(GetTodoQuery query)
        {
            var todo = await _repository.GetForUserAsync(query.TodoId, query.UserId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(query.TodoId);

            return _mapper.Map<DetailedTodoDto>(todo);
        }
    }
}