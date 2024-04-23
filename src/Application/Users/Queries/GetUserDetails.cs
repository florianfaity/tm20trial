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
    private readonly IPointsService _pointsService;

    public GetUserDetailsQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, IPointsService pointsService)
    {
        _context = context;
        _mapper = mapper;
        _identityService = identityService;
        _pointsService = pointsService;
    }

    public async Task<UserDetailsDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _identityService.FindUserByIdAsync(request.IdUser, cancellationToken);
        
        if (user == null || user.UserDetails == null)
        {
            throw new NotFoundException("GetCurrentUser","id");
        }

        var idUser = user.UserDetails.IdUser;
        var userDetailDto = _mapper.Map<UserDetailsDto>(user.UserDetails);

        var records = await _context.Records
            .Include(x => x.Map).Where(u => u.IdUser == idUser && u.IsValidated).ToListAsync(cancellationToken);
        
        userDetailDto.Records = _mapper.Map<IEnumerable<RecordDto>>(records);
        userDetailDto.Points = records.Sum(x => x.Map?.Points ?? 0);
        userDetailDto.NumberOfMapCompleted = records.Count;
        userDetailDto.NumberOfWorldRecord = 0;
        
        return userDetailDto;
    }
}
