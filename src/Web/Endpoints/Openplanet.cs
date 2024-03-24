using Microsoft.AspNetCore.Mvc;
using tm20trial.Application.Openplanet.Queries;

namespace tm20trial.Web.Endpoints;

public class Openplanet : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetAutorize)
            ;
    }
    
    public async Task<ContentResult> GetAutorize(ISender sender)
    {
        return await sender.Send(new GetAutorizeQuery());
    }
}
