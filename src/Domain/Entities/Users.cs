namespace tm20trial.Domain.Entities;

public partial class Users
{
    public required string DisplayName { get; set; }

    public required string LoginUplay { get; set; }

    public string? Nation { get; set; }

    public string? TwitchUsername { get; set; }

    public string? TwitterUsername { get; set; }
    
    public string? TmxId { get; set; }


    public virtual ICollection<Records> Records { get; set; }

     public Users(){
        
            Records = new HashSet<Records>();
     }
}
