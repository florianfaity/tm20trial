using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;
using tm20trial.Application.Records.Queries;

namespace tm20trial.Application.Users.Queries;

[Authorize(Policy = Constants.UserPolicies.ConnectedPolicy)]
public record GetUserDetailsQuery : IRequest<UserDetailsDto>
{
    public int IdUser {get;set;}
}

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;

    public GetUserDetailsQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService)
    {
        _context = context;
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<UserDetailsDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindUserByIdAsync(request.IdUser, cancellationToken);
        
        if (user == null || user.UserDetails == null)
        {
            throw new NotFoundException("GetCurrentUser","id");
        }
        
        var userDetailDto = _mapper.Map<UserDetailsDto>(user.UserDetails);

        var records = await _context.Records
            .Include(x => x.Map).Where(u => u.IdUser == user.UserDetails.IdUser && u.IsValidated).ToListAsync(cancellationToken);
        
        userDetailDto.Records = _mapper.Map<IEnumerable<RecordDto>>(records);
        
        return userDetailDto;
    }
}
