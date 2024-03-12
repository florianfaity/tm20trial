using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;
using tm20trial.Domain.Enums;

namespace tm20trial.Application.Maps.Queries;

public record GetMapsQuery : IRequest<IEnumerable<MapDto>>
{
}

public class GetMapsQueryHandler : IRequestHandler<GetMapsQuery, IEnumerable<MapDto>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMapsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MapDto>> Handle(GetMapsQuery request, CancellationToken cancellationToken)
    {
        var maps = await _context.Maps.Where(x => x.State == EStateValidation.Validate).Include(x => x.Records).ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<MapDto>>(maps);
    }
}
