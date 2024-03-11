using tm20trial.Application.Common.Exceptions;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;
using tm20trial.Domain.Interfaces;

namespace tm20trial.Application.Users.Command.CreateUser;

[Authorize(Policy = Constants.UserPolicies.AdministratorPolicy)]
public record CreateUserCommand : IRequest<int>
{
    
    public required string Email { get; set; }
    
    public required string DisplayName { get; set; }

    public string? LoginUplay { get; set; }

    public string? Nation { get; set; }

    public string? TwitchUsername { get; set; }

    public string? TwitterUsername { get; set; }

    public string? TmxId { get; set; }
    
    public bool Admin { get; set; }
    
    public bool Mapper { get; set; }
    
    public bool Player { get; set; }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty();
        RuleFor(v => v.DisplayName)
            .NotEmpty();
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IEncryptionService _encryptionService;

    public CreateUserCommandHandler(IApplicationDbContext context, IIdentityService identityService, IEncryptionService encryptionService)
    {
        _context = context;
        _identityService = identityService;
        _encryptionService = encryptionService;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExist = await _identityService.UserExistAsync(request.Email, cancellationToken);

        
        
        if (userExist)
        {
            throw new ConflictException("Email already exist.");
        }

        var user = new tm20trial.Domain.Entities.Users
        {
            DisplayName = request.DisplayName,
            LoginUplay = request.LoginUplay,
            Nation = request.Nation,
            TwitchUsername = request.TwitchUsername,
            TwitterUsername = request.TwitterUsername,
            TmxId = request.TmxId,
        };

        _context.Users.Add(user);

        List<string> roles = new List<string>();
        
        if (request.Admin)
            roles.Add(Constants.UserRoles.Administrator);
        if (request.Mapper || request.Admin)
            roles.Add(Constants.UserRoles.Mapper);
        if (request.Player || request.Admin || request.Mapper)
            roles.Add(Constants.UserRoles.Player);
        
        await _identityService.CreateUserAsync(request.Email, _encryptionService.GeneratePassword(), roles, user);
      
        await _context.SaveChangesAsync(cancellationToken);
        // Send mail to generate password
        return user.IdUser;
    }
}
