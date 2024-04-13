using tm20trial.Application.Records.Command.UpdateRecord;
using tm20trial.Application.Records.Queries;

namespace tm20trial.Web.Endpoints;

public class Records: EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPut(UpdateRecordByIoId, "{idMap}")
            .MapGet(GetRecords)
            .MapGet(GetRecordsMap, "{idMap}")
            ;
    }
    
    public async Task<IResult> UpdateRecordByIoId(ISender sender, int idMap)
    {
        await sender.Send(new UpdateRecordByIoIdCommand(idMap));
        return Results.NoContent();
    }
    
    public async Task<IEnumerable<RecordDto>> GetRecords(ISender sender)
    {
        return await sender.Send(new GetRecordsQuery());
    }
    
    public async Task<IEnumerable<RecordDto>> GetRecordsMap(ISender sender, int idMap)
    {
        return await sender.Send(new GetRecordsMapQuery{IdMap = idMap});
    }
}
