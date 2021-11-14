using AutoMapper;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Queries.Models.TodoTasks;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers.TodoTasks
{
    class GetTodoTaskQueryHandler : IQueryHandler<GetTodoTaskQuery, TodoTaskDto>
    {
        private readonly ITodoTaskRepository _repository;
        private readonly IMapper _mapper;

        public GetTodoTaskQueryHandler(ITodoTaskRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TodoTaskDto> HandleAsync(GetTodoTaskQuery query)
        {
            var task = await _repository.GetForUserAsync(query.TaskId, query.UserId);
            if (task is null)
                throw new TodoTaskWithGivenIdDoesNotExistException(query.TaskId);

            return _mapper.Map<TodoTaskDto>(task);
        }
    }
}