using Microsoft.Extensions.Caching.Memory;
using tm20trial.Application.Common.Exceptions;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Domain.Enums;

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
    private readonly IMemoryCache _memoryCache;
    private readonly ITrackmaniaService _trackmaniaService;
    

    public UpdateRecordByIoIdCommandHandler(IApplicationDbContext context, IIdentityService identityService, ICurrentUserService currentUser, ITrackmaniaService trackmaniaService, IMemoryCache memoryCache)
    {
        _context = context;
        _identityService = identityService;
        _currentUserService = currentUser;
        _trackmaniaService = trackmaniaService;
        _memoryCache = memoryCache;
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
        
        var record = await _trackmaniaService.GetUserRecordByMapId(map.TmIoId, user.UserDetails.TmIoId);

        if (record.RecordScore == null)
        {
            throw new NotFoundException("Record", "RecordScore");
        }
        
        var recordExist = await _context.Records.Where(x => x.IdMap == request.IdMap && x.IdUser == user.UserDetails.IdUser).FirstOrDefaultAsync(cancellationToken);;

        if (recordExist == null)
        {
            var newRecord = new Domain.Entities.Records
            {
                Time = TimeSpan.FromMilliseconds(record.RecordScore.Time),
                IdMap =  request.IdMap,
                IdUser = user.UserDetails.IdUser,
                FileUrl = record.Url,
                IsValidated = true,
                DatePersonalBest = record.Timestamp,
                Medal = (EMedal)record.Medal
            };
        
            await _context.Records.AddAsync(newRecord, cancellationToken);
        }
        else
        {
            if (recordExist.Time == TimeSpan.FromMilliseconds(record.RecordScore.Time))
                throw new ConflictException("Time not changed");//  return "Time not changed";
        
            recordExist.Time = TimeSpan.FromMilliseconds(record.RecordScore.Time);
            recordExist.FileUrl = record.Url;
            recordExist.DatePersonalBest = record.Timestamp;
            recordExist.Medal = (EMedal)record.Medal;
            _context.Records.Update(recordExist);
        }
        
        _memoryCache.Remove("leaderboard");
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}
