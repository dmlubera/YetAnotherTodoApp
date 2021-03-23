using System;

namespace YetAnotherTodoApp.Application.DTOs
{
    public class JwtDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}