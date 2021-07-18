using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Application.Mappers.Profiles
{
    public class StepProfile : Profile
    {
        public StepProfile()
        {
            CreateMap<Step, StepDto>()
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title.Value));
        }
    }
}