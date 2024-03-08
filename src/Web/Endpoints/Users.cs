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
            .MapGet(GetUsers)
            .MapGet(GetUsersRoles, "roles")
            .MapGet(GetCurrentUser, "current-user");
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
    
    public async Task<IEnumerable<string>> GetUsersRoles(ISender sender)
    {
        return await sender.Send(new GetUserRoles());
    }
    
    public async Task<CurrentUserDto> GetCurrentUser(ISender sender)
    {
        return await sender.Send(new GetCurrentUserQuery());
    }
}
