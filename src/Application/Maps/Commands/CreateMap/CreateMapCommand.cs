using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;
using tm20trial.Domain.Enums;

namespace tm20trial.Application.Maps.Commands.CreateMap;

[Authorize(Policy = Constants.UserPolicies.MapperPolicy)]
public record CreateMapCommand : IRequest<int>
{
    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public EDifficulty Difficulty { get; set; }
    public ETypeTrial TypeTrial { get; set; }
    public int Points { get; set; }
    public string? FileUrl { get; set; }
    public string? TmIoId { get; set; }
    public string? TmxLink { get; set; }
    public string? VideoLink { get; set; }
    public string? ImageLink { get; set; }
    public int NumberCheckpoint { get; set; }
}

public class CreateMapCommandValidator : AbstractValidator<CreateMapCommand>
{
    public CreateMapCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty();
        RuleFor(v => v.Author)
            .NotEmpty();
    }
}

public class CreateMapCommandHandler : IRequestHandler<CreateMapCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateMapCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMapCommand request, CancellationToken cancellationToken)
    {
        
        var entity = new tm20trial.Domain.Entities.Maps
        {
            Name = request.Name,
            Author = request.Author,
            Difficulty = request.Difficulty,
            TypeTrial = request.TypeTrial,
            Points = request.Points,
            FileUrl = request.FileUrl,
            TmIoId = request.TmIoId,
            TmxLink = request.TmxLink,
            VideoLink = request.VideoLink,
            ImageLink = request.ImageLink,
            NumberCheckpoint = request.NumberCheckpoint,
            State = EStateValidation.New,
        };

        _context.Maps.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
