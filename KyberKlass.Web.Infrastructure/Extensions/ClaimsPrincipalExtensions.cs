using System.Security.Claims;

namespace KyberKlass.Web.Infrastructure.Extensions;
public static class ClaimsPrincipalExtensions
{
    public static string GetId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}