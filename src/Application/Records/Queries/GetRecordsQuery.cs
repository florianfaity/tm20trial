using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Records.Queries;

[Authorize(Policy = Constants.UserPolicies.AdministratorPolicy)]
public record GetRecordsQuery: IRequest<IEnumerable<RecordDto>>
{
    
}

public class GetRecordsQueryHandler : IRequestHandler<GetRecordsQuery, IEnumerable<RecordDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRecordsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecordDto>> Handle(GetRecordsQuery request, CancellationToken cancellationToken)
    {
        var records = await _context.Records
            .Include(x => x.User)
            .Include(x => x.Map)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<RecordDto>>(records);
    }
}

