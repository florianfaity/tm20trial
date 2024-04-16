using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Users.Command.UpdateUserPassword;

[Authorize(Policy = Constants.UserPolicies.PlayerPolicy)]
public record UpdateUserPasswordCommand : IRequest
{
    public required string CurrentPassword { get; set; } 
    public required string NewPassword { get; set; } 
    public required string ConfirmPassword { get; set; } 
}


public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(v => v.CurrentPassword).NotEmpty();
        RuleFor(v => v.NewPassword).NotEmpty();
        RuleFor(v => v.ConfirmPassword).NotEmpty();
    }
}

public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;

    public UpdateUserPasswordCommandHandler(IIdentityService identityService, ICurrentUserService currentUserService)
    {
        _identityService = identityService;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        if(String.IsNullOrEmpty(_currentUserService.IdentityId))
        {
            throw new UnauthorizedAccessException();
        }
        
        var identityUser = await _identityService.FindUserByIdAsync(_currentUserService.IdentityId, cancellationToken);

        var result = await _identityService.UpdateUserPassword(identityUser, request.CurrentPassword, request.NewPassword, cancellationToken);

        if (!result.Succeeded)
        {
            throw new ValidationException("Fail to change password");
        }
    }
}
