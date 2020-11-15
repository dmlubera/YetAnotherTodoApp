using System;

namespace YetAnotherTodoApp.Infrastructure.Auth.DTO
{
    public class JwtDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}