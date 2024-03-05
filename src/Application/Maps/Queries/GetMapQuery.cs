using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Maps.Queries;

public record GetMapQuery : IRequest<MapDto>
{
    public int Id { get; set; }
}

public class GetMapQueryHandler : IRequestHandler<GetMapQuery, MapDto>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMapQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MapDto> Handle(GetMapQuery request, CancellationToken cancellationToken)
    {
        var maps = await _context.Maps.Include(x => x.Records).FirstOrDefaultAsync(m => m.Id == request.Id ,cancellationToken);
        return _mapper.Map<MapDto>(maps);
    }
}
