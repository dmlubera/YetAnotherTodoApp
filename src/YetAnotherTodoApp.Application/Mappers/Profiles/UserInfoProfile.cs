using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Application.Mappers.Profiles
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<User, UserInfoDto>()
                .ForMember(x => x.Username, opt => opt.MapFrom(x => x.Username.Value))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.Name.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.Name.LastName))
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email.Value));
        }
    }
}
