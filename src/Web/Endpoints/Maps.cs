using tm20trial.Application.Maps.Commands.CreateMap;
using tm20trial.Application.Maps.Commands.DeleteMap;
using tm20trial.Application.Maps.Commands.UpdateMap;
using tm20trial.Application.Maps.Queries;
using tm20trial.Domain.Enums;

namespace tm20trial.Web.Endpoints;

public class Maps : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetMap, "{id}")
            .MapGet(GetMaps, "state/{state}")
            .MapPost(CreateMap)
            .MapPut(UpdateMap, "{id}")
            .MapDelete(DeleteMap, "{id}")
            ;
    }
    
    public async Task<MapDto> GetMap(ISender sender, int id)
    {
        return await sender.Send(new GetMapQuery { Id = id });
    }
    
    public async Task<List<MapDto>> GetMaps(ISender sender, EStateValidation? state)
    {
        return await sender.Send(new GetMapsQuery { State = state });
    }
    
    public async Task<int> CreateMap(ISender sender, CreateMapCommand command)
    {
        return await sender.Send(command);
    }
    
    public async Task<IResult> UpdateMap(ISender sender, int id, UpdateMapCommand command)
    {
        if (id != command.Id) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
    
    public async Task<IResult> DeleteMap(ISender sender, int id)
    {
        await sender.Send(new DeleteMapCommand(id));
        return Results.NoContent();
    }
}
