using System.Threading.Tasks;
using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries.Models.Users;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers.Users
{
    public class GetUserInforQueryHandler : IQueryHandler<GetUserInfoQuery, UserInfoDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserInforQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserInfoDto> HandleAsync(GetUserInfoQuery query)
        {
            var user = await _userRepository.GetByIdAsync(query.UserId);
            return _mapper.Map<UserInfoDto>(user);
        }
    }
}