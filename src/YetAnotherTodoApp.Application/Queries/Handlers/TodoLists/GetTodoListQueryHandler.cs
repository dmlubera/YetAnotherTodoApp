using System.Threading.Tasks;
using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Queries.Models.TodoLists;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers.TodoLists
{
    public class GetTodoListQueryHandler : IQueryHandler<GetTodoListQuery, TodoListDto>
    {
        private readonly ITodoListRepository _repository;
        private readonly IMapper _mapper;

        public GetTodoListQueryHandler(ITodoListRepository repository, IMapper mapper)
            => (_repository, _mapper) = (repository, mapper);

        public async Task<TodoListDto> HandleAsync(GetTodoListQuery query)
        {
            var todoList = await _repository.GetForUserAsync(query.UserId, query.TodoListId);
            if (todoList is null)
                throw new TodoListWithGivenIdDoesNotExistException(query.TodoListId);

            return _mapper.Map<TodoListDto>(todoList);
        }
    }
}