using Microsoft.AspNetCore.Mvc;
using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Application.Openplanet.Queries;

public class GetAutorizeQuery: IRequest<ContentResult>
{
    
}

public class GetAutorizeQueryHandler : IRequestHandler<GetAutorizeQuery, ContentResult>
{
    private readonly ITrackmaniaService _trackmaniaService;

    public GetAutorizeQueryHandler(ITrackmaniaService trackmaniaService)
    {
        _trackmaniaService = trackmaniaService;
    }

    public async Task<ContentResult> Handle(GetAutorizeQuery request, CancellationToken cancellationToken)
    {
        return await _trackmaniaService.GetAutorize();
    }
}
