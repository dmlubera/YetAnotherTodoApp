using System.Threading.Tasks;
using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries.Models.Users;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Queries.Handlers.Users
{
    public class GetUserInforQueryHandler : IQueryHandler<GetUserInfoQuery, UserInfoDto>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public GetUserInforQueryHandler(IUserRepository repository, IMapper mapper)
            => (_repository, _mapper) = (repository, mapper);

        public async Task<UserInfoDto> HandleAsync(GetUserInfoQuery query)
            => _mapper.Map<UserInfoDto>(await _repository.GetByIdAsync(query.UserId));
    }
}