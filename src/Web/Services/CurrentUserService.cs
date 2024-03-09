using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection.Extensions;
using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    public string? IdentityId { get; }
    public int UserId { get; }
    
    public bool IsAdmin { get; set; }
    
    public bool IsMapper { get; set; }
    
    public bool IsPlayer { get; set; }
    
    public List<string> Roles { get; set; }
    

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        IdentityId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            UserId = httpContextAccessor.HttpContext?.User?.GetUserId() ?? 0;
            IsAdmin = httpContextAccessor.HttpContext?.User?.IsAdmin() ?? false;
            IsMapper = httpContextAccessor.HttpContext?.User?.IsMapper() ?? false;
            IsPlayer = httpContextAccessor.HttpContext?.User?.IsPlayer() ?? false;
            Roles = httpContextAccessor.HttpContext?.User?.GetRoles() ?? new List<string>();
        }
        catch (Exception)
        {
            UserId = 0;
            Roles = new List<string>();
        }
    }

   // public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

}
