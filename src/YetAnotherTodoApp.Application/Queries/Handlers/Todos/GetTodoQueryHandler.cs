using System.Threading.Tasks;
using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Queries.Models.Todos;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers.Todos
{
    public class GetTodoQueryHandler : IQueryHandler<GetTodoQuery, TodoDto>
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;

        public GetTodoQueryHandler(ITodoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TodoDto> HandleAsync(GetTodoQuery query)
        {
            var todo = await _repository.GetTodoAsync(query.TodoId);
            if (todo is null)
                throw new TodoWithGivenIdDoesNotExistException(query.TodoId);

            return _mapper.Map<TodoDto>(todo);
        }
    }
}