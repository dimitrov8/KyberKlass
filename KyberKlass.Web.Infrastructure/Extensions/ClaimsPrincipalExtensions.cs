#region

using System.Security.Claims;

#endregion

namespace KyberKlass.Web.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}