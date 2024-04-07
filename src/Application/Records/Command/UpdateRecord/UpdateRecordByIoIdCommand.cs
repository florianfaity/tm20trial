using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Application.Records.Command.UpdateRecord;

public record UpdateRecordByIoIdCommand(int IdMap) : IRequest;

public class UpdateRecordByIoIdCommandValidator : AbstractValidator<UpdateRecordByIoIdCommand>
{
    public UpdateRecordByIoIdCommandValidator()
    {
        RuleFor(v => v.IdMap)
            .NotNull();
    }
}

public class UpdateRecordByIoIdCommandHandler : IRequestHandler<UpdateRecordByIoIdCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ITrackmaniaService _trackmaniaService;
    

    public UpdateRecordByIoIdCommandHandler(IApplicationDbContext context, IIdentityService identityService, ICurrentUserService currentUser, ITrackmaniaService trackmaniaService)
    {
        _context = context;
        _identityService = identityService;
        _currentUserService = currentUser;
        _trackmaniaService = trackmaniaService;
    }
    
    public async Task Handle(UpdateRecordByIoIdCommand request, CancellationToken cancellationToken)
    {  
        if(String.IsNullOrEmpty(_currentUserService.IdentityId))
        {
            throw new UnauthorizedAccessException();
        }
        var user = await _identityService.FindUserByIdAsync(_currentUserService.IdentityId, cancellationToken);
        
        var map = await _context.Maps
            .Include(x => x.Records)
            .FirstOrDefaultAsync(x => x.Id == request.IdMap, cancellationToken);
        
        
        if(map == null || String.IsNullOrEmpty(map.TmIoId))
        {
            throw new NotFoundException("Map", "id");
        }

        if(user == null || user.UserDetails == null || String.IsNullOrEmpty(user.UserDetails.TmIoId))
        {
            throw new NotFoundException("User", "IoId");
        }
        
        var record = _trackmaniaService.GetUserRecordByMapId(map.TmIoId, user.UserDetails.TmIoId);

        var test = record;
            
   //     Guard.Against.NotFound(request.IdMap, record);
        
        
        await _context.SaveChangesAsync(cancellationToken);
        
    }
}
