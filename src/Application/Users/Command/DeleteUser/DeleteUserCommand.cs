using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Users.Command.DeleteUser;


[Authorize(Policy = Constants.UserPolicies.AdministratorPolicy)]
public record DeleteUserCommand(int Id) : IRequest;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull();
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUser;

    public DeleteUserCommandHandler(IApplicationDbContext context, IIdentityService identityService, ICurrentUserService currentUser)
    {
        _context = context;
        _identityService = identityService;
        _currentUser = currentUser;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {  
        var user = await _identityService.FindUserByIdAsync(request.Id
            , cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("DeleteUser","id");
        }
        
        if (_currentUser.IdentityId == user.Id)
        {
            throw new UnauthorizedAccessException();
        }
        
        var userDb = await _context.Users
            .Include(x => x.Records)
            .SingleOrDefaultAsync(u => u.IdUser == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, userDb);

        _context.Records.RemoveRange(userDb.Records);
        
        _context.Users.Remove(userDb);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
