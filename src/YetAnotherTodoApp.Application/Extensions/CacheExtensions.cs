using System;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Extensions
{
    public static class CacheExtensions
    {
        public static void SetJwt(this ICache cache, Guid tokenId, JwtDto jwtToken)
            => cache.Set(GenerateJwtKey(tokenId), jwtToken, TimeSpan.FromSeconds(99));

        public static JwtDto GetJwt(this ICache cache, Guid tokenId)
            => cache.Get<JwtDto>(GenerateJwtKey(tokenId));

        public static void SetId(this ICache cache, Guid tokenId, Guid id)
            => cache.Set(GenerateIdKey(tokenId), id, TimeSpan.FromSeconds(99));

        public static Guid GetId(this ICache cache, Guid tokenId)
            => cache.Get<Guid>(GenerateIdKey(tokenId));

        private static string GenerateJwtKey(Guid tokenId)
            => $"jwt-{tokenId}";

        private static string GenerateIdKey(Guid tokenId)
            => $"id-{tokenId}";
    }
}