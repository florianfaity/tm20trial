using tm20trial.Application.Records.Command.UpdateRecord;

namespace tm20trial.Web.Endpoints;

public class Records: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPut(UpdateRecordByIoId, "{idMap}")
            ;
    }
    
    public async Task<IResult> UpdateRecordByIoId(ISender sender, int idMap)
    {
        await sender.Send(new UpdateRecordByIoIdCommand(idMap));
        return Results.NoContent();
    }

}
