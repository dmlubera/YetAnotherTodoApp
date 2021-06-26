using System;

namespace YetAnotherTodoApp.Application.Cache
{
    public interface ICache
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expirationTime);
    }
}
