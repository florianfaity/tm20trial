using tm20trial.Application.Users.Queries;
using tm20trial.Domain.Constants;

namespace tm20trial.Web.Endpoints;

public class Auth : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetRoles);
    }
    
    public async Task<IEnumerable<string>> GetRoles(ISender sender)
    {
        return await sender.Send(new GetUserRoles());
    }
}
