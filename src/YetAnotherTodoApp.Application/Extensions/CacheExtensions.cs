using System;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Extensions
{
    public static class CacheExtensions
    {
        public static void SetJwtToken(this ICache cache, Guid tokenId, JwtDto jwtToken)
            => cache.Set(tokenId.ToString(), jwtToken, TimeSpan.FromSeconds(60));

        public static JwtDto GetJwtToken(this ICache cache, Guid tokenId)
            => cache.Get<JwtDto>(tokenId.ToString());

        public static void SetResourceIdentifier(this ICache cache, Guid tokenId, Guid id)
            => cache.Set(tokenId.ToString(), id, TimeSpan.FromSeconds(60));

        public static Guid GetResourceIdentifier(this ICache cache, Guid tokenId)
            => cache.Get<Guid>(tokenId.ToString());
    }
}