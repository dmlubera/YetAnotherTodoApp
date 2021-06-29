using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Application.Mappers.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Todo, TodoDto>()
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title.Value))
                .ForMember(x => x.FinishDate, opt => opt.MapFrom(x => x.FinishDate.Value));
        }
    }
}
