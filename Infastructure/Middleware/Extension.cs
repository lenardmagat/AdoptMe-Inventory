using System.Security.Claims;

namespace inventory.Extensions;

public static class ClaimsPrincipalExtensions
{
    // 💡 Changed the receiver to "this ClaimsPrincipal principal"
    public static int? GetUserId(this ClaimsPrincipal principal)
    {
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        // Look for the ID claims directly on the principal
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) 
                       ?? principal.FindFirst("id") 
                       ?? principal.FindFirst("sub");

        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }

        return null;
    }
}