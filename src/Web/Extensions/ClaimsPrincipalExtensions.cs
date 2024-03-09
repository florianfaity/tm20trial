using System.Security.Claims;
using tm20trial.Application.Common.Models;

namespace Microsoft.Extensions.DependencyInjection.Extensions;

public  static class ClaimsPrincipalExtensions
{
    
    public static bool IsAdmin(this ClaimsPrincipal principal)
    {
        return HasRole(principal, Constants.UserRoles.Administrator);
    }
 
    public static bool IsPlayer(this ClaimsPrincipal principal)
    {
        return HasRole(principal, Constants.UserRoles.Player);
    }
    
    public static bool IsMapper(this ClaimsPrincipal principal)
    {
        return HasRole(principal, Constants.UserRoles.Mapper);
    }
    
    public static List<string> GetRoles(this ClaimsPrincipal principal)
    {
        return principal.FindAll(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
    }

    public static int GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var id = principal.FindFirstValue(Constants.UserCustomClaims.UserId);

        if (string.IsNullOrEmpty(id))
        {
            return 0;
            //throw new AuthenticationException();
        }

        return int.Parse(id);
    }
    
    private static bool HasRole(this ClaimsPrincipal principal, string roleName)
    {
        ArgumentNullException.ThrowIfNull(principal);

        return principal.HasClaim(ClaimTypes.Role, roleName);
    }
}
