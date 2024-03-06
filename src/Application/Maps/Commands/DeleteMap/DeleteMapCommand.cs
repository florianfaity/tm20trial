using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Maps.Commands.DeleteMap;

[Authorize(Policy = Constants.UserPolicies.AdministratorPolicy)]
public record DeleteMapCommand(int Id) : IRequest;

public class DeleteMapCommandValidator : AbstractValidator<DeleteMapCommand>
{
    public DeleteMapCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull();
    }
}

public class DeleteMapCommandHandler : IRequestHandler<DeleteMapCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteMapCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMapCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Maps
            .FindAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        _context.Maps.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
