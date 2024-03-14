using System.ComponentModel.DataAnnotations;

namespace tm20trial.Domain.Entities;

public class Users
{
    [Key]
    public int IdUser { get; set; }
    
    public string? DisplayName { get; set; }

    public string? LoginUplay { get; set; }

    public string? Nation { get; set; }

    public string? TwitchUsername { get; set; }

    public string? TwitterUsername { get; set; }

    public string? TmxId { get; set; }
    
    public string? TmIoId { get; set; }

    public virtual ICollection<Records> Records { get; set; } = new HashSet<Records>();
}
