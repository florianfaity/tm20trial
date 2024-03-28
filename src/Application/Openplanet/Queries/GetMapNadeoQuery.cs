using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Application.Openplanet.Queries;

public record GetMapNadeoQuery : IRequest<NadeoMapDto>
{
    public required string MapId { get; set; }
}

public class GetMapNadeoQueryHandler : IRequestHandler<GetMapNadeoQuery, NadeoMapDto>
{
    private readonly IMapper _mapper;
    private readonly ITrackmaniaService _trackmaniaService;

    public GetMapNadeoQueryHandler(IMapper mapper, ITrackmaniaService trackmaniaService)
    {
        _mapper = mapper;
        _trackmaniaService = trackmaniaService;
    }

    public async Task<NadeoMapDto> Handle(GetMapNadeoQuery request, CancellationToken cancellationToken)
    {
        var mapNadeo = await _trackmaniaService.GetMapById(request.MapId);
        
        
        return _mapper.Map<NadeoMapDto>(mapNadeo);
    }
}
