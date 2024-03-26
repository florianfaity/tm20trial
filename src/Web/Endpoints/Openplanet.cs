using Microsoft.AspNetCore.Mvc;
using tm20trial.Application.Openplanet.Queries;

namespace tm20trial.Web.Endpoints;

public class Openplanet : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(SearchNadeoMap, "{mapId}")
            ;
    }
    
    public async Task<NadeoMapDto> SearchNadeoMap(ISender sender, string mapId)
    {
        return await sender.Send(new GetMapNadeoQuery { MapId = mapId });
    }
    
}
