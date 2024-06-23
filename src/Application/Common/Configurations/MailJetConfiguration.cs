using tm20trial.Application.Common.Models;

namespace tm20trial.Application.Common.Configurations;

public class MailJetConfiguration
{
    public string? DevelopmentEmail { get; set; }

    public required string ApiKeyPublic { get; set; }

    public required string ApiKeyPrivate { get; set; }

    public required EmailPerson From { get; set; }
}
