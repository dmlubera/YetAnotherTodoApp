using System;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Helpers
{
    public interface IJwtHelper
    {
        JwtDto GenerateJwtToken(Guid userId);
    }
}