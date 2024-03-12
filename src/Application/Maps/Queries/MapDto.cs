using tm20trial.Domain.Entities;
using tm20trial.Domain.Enums;

namespace tm20trial.Application.Maps.Queries;

public class MapDto
{
    public int IdMap { get; set; }

    public string? Name { get; set; }

    public string? Author { get; set; }

    public EDifficulty Difficulty { get; set; }

    public ETypeTrial TypeTrial { get; set; }

    public int Points { get; set; }

    public string? TmxLink { get; set; }

    public string? VideoLink { get; set; }

    public string? ImageLink { get; set; }

    public int NumberCheckpoint { get; set; }

    public int NumberFinisher { get; set; }

    public TimeSpan? BestTime { get; set; }

    public EStateValidation State { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Maps, MapDto>()
                .ForMember(d => d.NumberFinisher, opt => opt.MapFrom(s => s.Records.Count()))
                .ForMember(d => d.BestTime, opt => opt.MapFrom(s => s.Records.Any() ? (TimeSpan?)s.Records.Min(r => r.Time) : null));
        }
    }
}
