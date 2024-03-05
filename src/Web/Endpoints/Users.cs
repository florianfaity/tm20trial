using Microsoft.AspNetCore.Authorization;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Users.Queries;

namespace tm20trial.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetUser, "{id}")
            .MapGet(GetUsers);
    }
    
    public async Task<UserDto> GetUser(ISender sender, int id)
    {
        return await sender.Send(new GetUserQuery { IdUser = id });
    }
    
    [Authorize(Policy = Constants.UserPolicies.AdministratorPolicy)]
    public async Task<IEnumerable<UserDto>> GetUsers(ISender sender)
    {
        return await sender.Send(new GetUsersQuery());
    }
}
