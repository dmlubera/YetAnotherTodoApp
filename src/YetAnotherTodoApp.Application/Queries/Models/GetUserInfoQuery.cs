using System;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Queries.Models
{
    public class GetUserInfoQuery : IQuery<UserInfoDto>
    {
        public Guid UserId { get; set; }
    }
}
