using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Users.Queries;

[Authorize(Policy = Constants.UserPolicies.AdministratorPolicy)]
public record GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
    
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        if(_currentUserService.IsAdmin)
        {
            return await _context.Users
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        
        throw new UnauthorizedAccessException();
    }
}
