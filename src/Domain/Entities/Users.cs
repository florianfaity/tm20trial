namespace tm20trial.Domain.Entities;

public class Users
{
    public Users()
    {
        Records = new HashSet<Records>();
    }
    
    public string? DisplayName { get; set; } 

    public string? LoginUplay { get; set; } 

    public string? Nation { get; set; }

    public string? TwitchUsername { get; set; }

    public string? TwitterUsername { get; set; }

    public string? TmxId { get; set; }
    
    public virtual ICollection<Records> Records { get; set; }

}
