using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Application.Records.Command.UpdateStateRecord;

public record UpdateStateRecordCommand : IRequest
{
    public int Id { get; set; }
    
    public bool Validate { get; set; }
}

public class UpdateStateRecordCommandValidator : AbstractValidator<UpdateStateRecordCommand>
{
    public UpdateStateRecordCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull();
        RuleFor(v => v.Validate)
            .NotNull();
    }
}

public class UpdateStateRecordCommandHandler : IRequestHandler<UpdateStateRecordCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateStateRecordCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateStateRecordCommand request, CancellationToken cancellationToken)
    {
        var record = await _context.Records
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (record == null)
        {
            throw new NotFoundException(nameof(record), request.Id.ToString());
        }

        record.IsValidated = request.Validate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
