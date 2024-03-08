﻿namespace tm20trial.Application.Users.Queries;

public class CurrentUserDto
{
    public int IdUser { get; set; }

    public string? DisplayName { get; set; }

    public string? LoginUplay { get; set; }

    public string? Nation { get; set; }

    public string? TwitchUsername { get; set; }

    public string? TwitterUsername { get; set; }

    public string? TmxId { get; set; }

    public List<string> Roles { get; set; } = new List<string>();
    
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Users, UserDto>();
        }
    }
}
