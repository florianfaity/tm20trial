namespace tm20trial.Application.Common.Models;

public class UbiServicesBearerTokenResponse
{
    public string? token_type { get; set; }
    
    public decimal? expires_in { get; set; }
    
    public string? access_token { get; set; }
}
