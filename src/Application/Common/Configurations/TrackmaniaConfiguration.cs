namespace tm20trial.Application.Common.Configurations;

public class TrackmaniaConfiguration
{
    public required string ClientId { get; set; }
    
    // public required string ClientSecret { get; set; }

    public required string BaseLoginURL { get; set; }

    public required string BaseAPIURL { get; set; }
}
