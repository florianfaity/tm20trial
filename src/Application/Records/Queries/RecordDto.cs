using tm20trial.Domain.Enums;

namespace tm20trial.Application.Records.Queries;

public class RecordDto  
{
    public bool IsValidated { get; set; }

    public TimeSpan Time { get; set; }
    
    public DateTime DatePersonalBest { get; set; }

    public string? MapName { get; set; }

    public string? DisplayName { get; set; }
    
    public string? FileUrl { get; set; }
    
    public EMedal Medal { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Records, RecordDto>()
                .ForMember(d => d.MapName, opt => opt.MapFrom(s => s.Map != null ? s.Map.Name : ""))
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(s => s.User != null ? s.User.DisplayName: ""));

        }
    }
}
