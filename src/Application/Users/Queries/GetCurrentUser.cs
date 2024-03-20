using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Users.Queries;

// [Authorize(Policy = Constants.UserPolicies.ConnectedPolicy)]
public record GetCurrentUserQuery : IRequest<CurrentUserDto>
{
}

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;

    public GetCurrentUserQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _identityService = identityService;
        _currentUserService = currentUserService;
    }

    public async Task<CurrentUserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if(String.IsNullOrEmpty(_currentUserService.IdentityId))
        {
            throw new UnauthorizedAccessException();
        }
        var user = await _identityService.FindUserByIdAsync(_currentUserService.IdentityId, cancellationToken);
        
        if (user == null || user.UserDetails == null)
        {
            throw new NotFoundException("GetCurrentUser","id");
        }
        
        var userDto = _mapper.Map<CurrentUserDto>(user.UserDetails);
        
        userDto.Roles =_currentUserService.Roles;

        return userDto;
    }
}
