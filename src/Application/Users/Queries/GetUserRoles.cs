using System.Security.Claims;
using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Application.Users.Queries;

public class GetUserRoles : IRequest<IEnumerable<string>>
{
    
}

public class GetUserRolesHandler : IRequestHandler<GetUserRoles, IEnumerable<string>>
{
    private readonly ICurrentUserService _currentUserService;

    public GetUserRolesHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<string>> Handle(GetUserRoles request, CancellationToken cancellationToken)
    {
    //    var res = _currentUserService._httpContextAccessor?.HttpContext?.User?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList() ?? new List<string>();
        var test = _currentUserService.Roles;
        return await Task.FromResult(_currentUserService.Roles);
    }
}
