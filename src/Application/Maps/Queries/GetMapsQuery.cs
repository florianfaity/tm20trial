using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;
using tm20trial.Domain.Enums;

namespace tm20trial.Application.Maps.Queries;

public record GetMapsQuery : IRequest<List<MapDto>>
{
    public EStateValidation? State { get; set; }
}

public class GetMapsQueryHandler : IRequestHandler<GetMapsQuery, List<MapDto>>
{

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMapsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MapDto>> Handle(GetMapsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Maps.AsQueryable().Where(x => x.TypeTrial == ETypeTrial.Classic);

        if (request.State != null)
        {
            query = query.Where(x => x.State == request.State);
        }

        var listMap = await query.Include(x => x.Records).ToListAsync(cancellationToken);
        return _mapper.Map<List<MapDto>>(listMap);
    }
}
