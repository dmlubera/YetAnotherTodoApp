using AutoMapper;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Queries.Models;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers
{
    public class GetTodoListQueryHandler : IQueryHandler<GetTodoListQuery, TodoListDto>
    {
        private readonly ITodoListRepository _repository;
        private readonly IMapper _mapper;

        public GetTodoListQueryHandler(ITodoListRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TodoListDto> HandleAsync(GetTodoListQuery query)
        {
            var todoList = await _repository.GetForUserAsync(query.UserId, query.TodoListId);
            if (todoList is null)
                throw new TodoListWithGivenIdDoesNotExistException(query.TodoListId);

            return _mapper.Map<TodoListDto>(todoList);
        }
    }
}