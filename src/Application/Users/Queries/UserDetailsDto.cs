using tm20trial.Application.Common.Mappings;
using tm20trial.Application.Records.Queries;
using tm20trial.Domain.Entities;

namespace tm20trial.Application.Users.Queries;

public class UserDetailsDto : IMapFrom<Domain.Entities.Users>
{
    public int IdUser { get; set; }

    public string? DisplayName { get; set; }

    public string? LoginUplay { get; set; }

    public string? Nation { get; set; }

    public string? TwitchUsername { get; set; }

    public string? TwitterUsername { get; set; }

    public string? TmxId { get; set; }
    
    public string? TmIoId { get; set; }
    
    public int Points { get; set; }
    
    public int NumberOfWorldRecord { get; set; }
    
    public int NumberOfMapCompleted { get; set; }
    
    public IEnumerable<RecordDto>? Records { get; set; } 
    
    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<Domain.Entities.Users, UserDetailsDto>();
    }

}
