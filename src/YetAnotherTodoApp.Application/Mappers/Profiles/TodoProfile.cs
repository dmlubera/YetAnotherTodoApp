using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Application.Mappers.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Todo, TodoDto>();
        }
    }
}
