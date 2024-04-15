using tm20trial.Application.Common.Mappings;

namespace tm20trial.Application.Users.Queries;

public class CurrentUserDto : IMapFrom<Domain.Entities.Users>
{
    public int IdUser { get; set; }

    public string? DisplayName { get; set; }

    public string? LoginUplay { get; set; }

    public string? Nation { get; set; }

    public string? TwitchUsername { get; set; }

    public string? TwitterUsername { get; set; }

    public string? TmxId { get; set; }
    
    public string? TmIoId { get; set; }

    public string? Email { get; set; }
    
    public List<string> Roles { get; set; } = new List<string>();
    
    
    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<Domain.Entities.Users, CurrentUserDto>();
    }
}
