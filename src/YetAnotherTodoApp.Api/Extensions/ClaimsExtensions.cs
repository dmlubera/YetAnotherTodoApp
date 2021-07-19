using System;
using System.Security.Claims;

namespace YetAnotherTodoApp.Api.Extensions
{
    public static class ClaimsExtensions
    {
        public static Guid GetAuthenticatedUserId(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.Identity.IsAuthenticated
                ? Guid.Parse(claimsPrincipal.Identity.Name)
                : Guid.Empty;
    }
}