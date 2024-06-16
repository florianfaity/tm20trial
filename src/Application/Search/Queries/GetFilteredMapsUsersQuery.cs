using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Maps.Queries;
using tm20trial.Application.Users.Queries;

namespace tm20trial.Application.Search.Queries;

public class GetFilteredMapsUsersQuery : IRequest<ListSearchDto>
{
    public required string Search { get; set; } 
}



public class GetFilteredMapsUsersQueryHandler : IRequestHandler<GetFilteredMapsUsersQuery, ListSearchDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    
    public GetFilteredMapsUsersQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService)
    {
        _context = context;
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<ListSearchDto> Handle(GetFilteredMapsUsersQuery request, CancellationToken cancellationToken)
    {
        var maps = new List<Domain.Entities.Maps>();
        var users =  new List<Domain.Entities.Users>();
        if (!string.IsNullOrEmpty(request.Search))
        {
            maps = await _context.Maps
                .Where(m => m.Name.Contains(request.Search) || m.Author.Contains(request.Search) 
                || m.Name.ToLower().Contains(request.Search.ToLower()) || m.Author.ToLower().Contains(request.Search.ToLower())).ToListAsync(cancellationToken);
            users = await _context.Users
                .Where(m => m.DisplayName != null && (m.DisplayName.Contains(request.Search) || m.DisplayName.ToLower().Contains(request.Search.ToLower()))).ToListAsync(cancellationToken);
        }
        
        return new ListSearchDto
        {
            Maps= _mapper.Map<List<MapDto>>(maps),
            Users = _mapper.Map<List<UserDto>>(users)
        };
    }
}
