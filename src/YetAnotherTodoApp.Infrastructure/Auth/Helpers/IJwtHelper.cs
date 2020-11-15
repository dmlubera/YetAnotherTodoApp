using System;
using YetAnotherTodoApp.Infrastructure.Auth.DTO;

namespace YetAnotherTodoApp.Infrastructure.Auth.Helpers
{
    public interface IJwtHelper
    {
        JwtDto GenerateJwtToken(Guid userId);
    }
}