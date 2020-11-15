namespace YetAnotherTodoApp.Infrastructure.Auth.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public int ExpiryTimeInMinutes { get; set; }
    }
}