using tm20trial.Application.Leaderboard.Queries;
using tm20trial.Application.Users.Queries;

namespace tm20trial.Web.Endpoints;

public class Leaderboards: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetLeaderboard)
            ;
    }
    
    public async Task<List<UserDetailsDto>> GetLeaderboard(ISender sender)
    {
        return await sender.Send(new GetLeaderboardQuery());
    }
}
