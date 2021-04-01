using System;
using Microsoft.Extensions.Caching.Memory;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Application.Extensions
{
    public static class CacheExtensions
    {
        public static void SetJwt(this IMemoryCache cache, Guid tokenId, JwtDto jwtToken)
            => cache.Set(GenerateJwtKey(tokenId), jwtToken, TimeSpan.FromSeconds(5));

        public static JwtDto GetJwt(this IMemoryCache cache, Guid tokenId)
            => cache.Get(GenerateJwtKey(tokenId)) as JwtDto;

        private static string GenerateJwtKey(Guid tokenId)
            => $"jwt-{tokenId}";
    }
}