using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Mappers.Profiles
{
    public class StepRequestDtoProfile : Profile
    {
        public StepRequestDtoProfile()
            => CreateMap<StepRequestDto, StepDto>();
    }
}