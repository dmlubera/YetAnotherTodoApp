using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Application.Mappers.Profiles
{
    public class TodoTaskProfile : Profile
    {
        public TodoTaskProfile()
        {
            CreateMap<TodoTask, TodoTaskDto>()
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title.Value));
        }
    }
}