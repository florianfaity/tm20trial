using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;
using tm20trial.Domain.Enums;

namespace tm20trial.Application.Maps.Commands.UpdateMap;


[Authorize(Policy = Constants.UserPolicies.AdministratorPolicy)]
public record UpdateMapCommand: IRequest
{
    public int Id { get; set; }
    
    public string Name { get; set; } = "";
    
    public string Author { get; set; } = "";
    
    public EDifficulty Difficulty { get; set; }
    
    public ETypeTrial TypeTrial { get; set; }
    
    public int Points { get; set; }
    
    public string? TmIoId { get; set; }
    
    public string? TmxLink { get; set; }
    
    
    public string? VideoLink { get; set; }
    
    public string? ImageLink { get; set; }
    
    public int NumberCheckpoint { get; set; }
    
    public bool Validate { get; set; }
}

public class UpdateMapCommandValidator : AbstractValidator<UpdateMapCommand>
{
    public UpdateMapCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull();
        RuleFor(v => v.Name)
            .NotEmpty();
        RuleFor(v => v.Author)
            .NotEmpty();
    }
}

public class UpdateMapCommandHandler : IRequestHandler<UpdateMapCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMapCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMapCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Maps
            .FindAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Name = request.Name;
        entity.Author = request.Author;
        entity.Difficulty = request.Difficulty;
        entity.TypeTrial = request.TypeTrial;
        entity.Points = request.Points;
        entity.TmIoId = request.TmIoId;
        entity.TmxLink = request.TmxLink;
        entity.VideoLink = request.VideoLink;
        entity.ImageLink = request.ImageLink;
        entity.NumberCheckpoint = request.NumberCheckpoint;
        entity.Validate = request.Validate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
