

using Microsoft.Extensions.Caching.Memory;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Records.Queries;
using tm20trial.Application.Users.Queries;

namespace tm20trial.Application.Leaderboard.Queries;

public class GetLeaderboardQuery : IRequest<List<UserDetailsDto>>
{
    
}


public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, List<UserDetailsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identityService;
    private readonly IPointsService _pointsService;
    private const string KeyCachingLeaderboardToken = "leaderboard";
    private readonly MemoryCacheEntryOptions _memoryCacheOptionsToken;
    private readonly IMemoryCache _memoryCache;

    public GetLeaderboardQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identityService, IPointsService pointsService, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _identityService = identityService;
        _pointsService = pointsService;
        _memoryCache = memoryCache;
        _memoryCacheOptionsToken = new MemoryCacheEntryOptions();
        _memoryCacheOptionsToken.SetSlidingExpiration(TimeSpan.FromMinutes(120)); 
    }

    public async Task<List<UserDetailsDto>> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var listUserLeaderboard = new List<UserDetailsDto>();
        var cacheExist = _memoryCache.TryGetValue(KeyCachingLeaderboardToken, out listUserLeaderboard);

        if (cacheExist && listUserLeaderboard != null)
        {
            return listUserLeaderboard;
        }
        listUserLeaderboard = new List<UserDetailsDto>();
        
        var users = await _context.Users
            .Include(u => u.Records)
            .ThenInclude(x => x.Map)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            var records = user.Records;
            var userDetailDto = _mapper.Map<UserDetailsDto>(user);
            userDetailDto.Records = null;
            userDetailDto.Points = records.Sum(x => x.Map?.Points ?? 0);
            userDetailDto.NumberOfMapCompleted =  records.Count;
            userDetailDto.NumberOfWorldRecord = 0;
            listUserLeaderboard.Add(userDetailDto);
        }

        listUserLeaderboard = listUserLeaderboard.OrderByDescending(x => x.Points).ToList();
        
        _memoryCache.Set(KeyCachingLeaderboardToken, listUserLeaderboard, _memoryCacheOptionsToken);

        return listUserLeaderboard;
    }
}
