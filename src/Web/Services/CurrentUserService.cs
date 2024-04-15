using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection.Extensions;
using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    public string? IdentityId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    
    public int UserId  => _httpContextAccessor.HttpContext?.User?.GetUserId() ?? 0;
    
    public bool IsAdmin => _httpContextAccessor.HttpContext?.User?.IsAdmin() ?? false;
    
    public bool IsMapper => _httpContextAccessor.HttpContext?.User?.IsMapper() ?? false;
    
    public bool IsPlayer => _httpContextAccessor.HttpContext?.User?.IsPlayer() ?? false;
    
    public List<string> Roles => _httpContextAccessor.HttpContext?.User?.GetRoles() ?? new List<string>();
    

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

}
