using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Users.Command.UpdateUserPassword;

[Authorize(Policy = Constants.UserPolicies.PlayerPolicy)]
public record UpdateUserPasswordCommand : IRequest
{
    public required string Password { get; set; }
}


public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(v => v.Password).NotEmpty();
    }
}

