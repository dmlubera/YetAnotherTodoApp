namespace YetAnotherTodoApp.Application.Helpers
{
    public interface IEncrypter
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}