using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;
using tm20trial.Domain.Enums;

namespace tm20trial.Application.Maps.Commands.UpdateStateMap;

[Authorize(Policy = Constants.UserPolicies.AdministratorPolicy)]
public record UpdateStateMapCommand: IRequest
{
    public int Id { get; set; }
    
    public EStateValidation State { get; set; }
}

public class UpdateStateMapCommandValidator : AbstractValidator<UpdateStateMapCommand>
{
    public UpdateStateMapCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull();
        RuleFor(v => v.State)
            .NotNull();
    }
}

public class UpdateStateMapCommandHandler : IRequestHandler<UpdateStateMapCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStateMapCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateStateMapCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Maps
            .FindAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.State = request.State;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
