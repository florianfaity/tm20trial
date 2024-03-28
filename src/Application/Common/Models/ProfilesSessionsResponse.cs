namespace tm20trial.Application.Common.Models;

public class ProfilesSessionsResponse
{
    public string? platformType { get; set; }
    
    public string? ticket { get; set; }
    
    public string? twoFactorAuthenticationTicket { get; set; }
    public string? profileId { get; set; }
    public string? userId { get; set; }
    public string? nameOnPlatform { get; set; }
    public string? environment { get; set; }
    public DateTime expiration { get; set; }
    public string? spaceId { get; set; }
    public string? clientIp { get; set; }
    public string? clientIpCountry { get; set; }
    public DateTime serverTime { get; set; }
    public string? sessionId { get; set; }
    public string? sessionKey { get; set; }
    public string? rememberMeTicket { get; set; }
}
