using System;
using Microsoft.Extensions.Caching.Memory;
using YetAnotherTodoApp.Application.DTOs;

namespace YetAnotherTodoApp.Infrastructure.Auth.Extensions
{
    public static class CacheExtensions
    {
        public static void SetJwt(this IMemoryCache cache, Guid tokenId, JwtDto jwtToken)
            => cache.Set(GenerateJwtKey(tokenId), jwtToken, TimeSpan.FromSeconds(5));

        public static void GetJwt(this IMemoryCache cache, Guid tokenId)
            => cache.Get(GenerateJwtKey(tokenId));

        private static string GenerateJwtKey(Guid tokenId)
            => $"jwt-{tokenId}";
    }
}