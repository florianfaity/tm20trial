using tm20trial.Application.Search.Queries;

namespace tm20trial.Web.Endpoints;

public class Search: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetFilteredMapsUsers,"{search}");
    }
    
    public async Task<ListSearchDto> GetFilteredMapsUsers(ISender sender, string search)
    {
        return await sender.Send(new GetFilteredMapsUsersQuery { Search = search});
    }
}
