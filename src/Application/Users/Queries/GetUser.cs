using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Users.Queries;

[Authorize(Policy = Constants.UserPolicies.ConnectedPolicy)]
public record GetUserQuery : IRequest<UserDto>
{
    public int IdUser {get;set;}
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;

    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService)
    {
        _context = context;
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindUserByIdAsync(request.IdUser, cancellationToken);
        
        if (user == null || user.UserDetails == null)
        {
            throw new NotFoundException("GetCurrentUser","id");
        }
        
        return  _mapper.Map<UserDto>(user.UserDetails);
    }
}
