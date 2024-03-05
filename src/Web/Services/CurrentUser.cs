using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection.Extensions;
using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Web.Services;

public class CurrentUser : ICurrentUserService
{
    public string? IdentityId { get; }
    public int UserId { get; }
    
    public bool IsAdmin { get; set; }
    
    public List<string> Roles { get; set; }
    
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        
        IdentityId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            UserId = httpContextAccessor.HttpContext?.User?.GetUserId() ?? 0;
            IsAdmin = httpContextAccessor.HttpContext?.User?.IsAdmin() ?? false;
            Roles = httpContextAccessor.HttpContext?.User?.GetRoles() ?? new List<string>();
        }
        catch (Exception)
        {
            UserId = 0;
            Roles = new List<string>();
        }
    }

    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

}
