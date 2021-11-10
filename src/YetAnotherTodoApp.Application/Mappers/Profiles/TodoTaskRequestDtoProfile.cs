using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Mappers.Profiles
{
    public class TodoTaskRequestDtoProfile : Profile
    {
        public TodoTaskRequestDtoProfile()
            => CreateMap<TodoTaskRequestDto, TodoTaskDto>();
    }
}