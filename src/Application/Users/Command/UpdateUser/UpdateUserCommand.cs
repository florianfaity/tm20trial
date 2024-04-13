using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Users.Command.UpdateUser;

[Authorize(Policy = Constants.UserPolicies.PlayerPolicy)]
public record UpdateUserCommand : IRequest
{
    public required int Id { get; set; }
    public required string DisplayName { get; set; }

    public string? LoginUplay { get; set; }

    public string? Nation { get; set; }

    public string? TwitchUsername { get; set; }

    public string? TwitterUsername { get; set; }

    public string? TmxId { get; set; }
    
    public required string TmIoId { get; set; }
    
    public required string Email { get; set; }
    
    public bool IsMapper { get; set; }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.DisplayName).NotEmpty();
        RuleFor(v => v.TmIoId).NotEmpty();
        RuleFor(f => f.Email).NotEmpty();
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUser;
    
    public UpdateUserCommandHandler(IApplicationDbContext context, IIdentityService identityService, ICurrentUserService currentUser)
    {
        _context = context;
        _identityService = identityService;
        _currentUser = currentUser;
    }
    
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserId != request.Id && !_currentUser.IsAdmin)
        {
            throw new UnauthorizedAccessException();
        }
        
        var user = await _identityService.FindUserByIdAsync(request.Id, cancellationToken);
        
        if (user == null || user.UserDetails == null)
        {
            throw new NotFoundException("UpdateUser", "id");
        }           
        
        var userDb = await _context.Users
            .FirstOrDefaultAsync(x => x.IdUser == user.UserDetails.IdUser, cancellationToken);

        if (userDb == null)
        {
            throw new NotFoundException("UpdateUserdb", "id");
        }
        
        userDb.DisplayName = request.DisplayName;
        userDb.LoginUplay = request.LoginUplay;
        userDb.Nation = request.Nation;
        userDb.TwitchUsername = request.TwitchUsername;
        userDb.TwitterUsername = request.TwitterUsername;
        userDb.TmIoId = request.TmIoId;
        userDb.TmxId = request.TmxId;
        
        _context.Users.Update(userDb);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        user.Email = request.Email;
        user.UserDetails = userDb;
        
        await _identityService.UpdateUserAsync(user, cancellationToken);

        if (request.IsMapper)
        {
            await _identityService.AddUserRoleAsync(user.Id, Constants.UserRoles.Mapper, cancellationToken);
        }
        else
        {
            await _identityService.DeleteUserRoleAsync(user.Id, Constants.UserRoles.Mapper, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
