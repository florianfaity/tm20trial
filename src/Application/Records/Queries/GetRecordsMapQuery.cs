using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Application.Records.Queries;

public record GetRecordsMapQuery: IRequest<IEnumerable<RecordDto>>
{
    public int IdMap { get; set; }
}

public class GetRecordsMapQueryValidator: AbstractValidator<GetRecordsMapQuery>
{
    public GetRecordsMapQueryValidator()
    {
        RuleFor(v => v.IdMap).NotNull();
    }
}

public class GetRecordsMapQueryHandler: IRequestHandler<GetRecordsMapQuery, IEnumerable<RecordDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRecordsMapQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
        
    public async Task<IEnumerable<RecordDto>> Handle(GetRecordsMapQuery request, CancellationToken cancellationToken)
    {
        var records = await _context.Records
            .Include(x => x.User)
            .Include(x => x.Map)
            .Where(x => x.IdMap == request.IdMap && x.IsValidated)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<RecordDto>>(records);
    }
}
