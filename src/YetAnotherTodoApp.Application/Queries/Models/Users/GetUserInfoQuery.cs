using System;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Queries.Models.Users
{
    public class GetUserInfoQuery : IQuery<UserInfoDto>
    {
        public Guid UserId { get; set; }
    }
}