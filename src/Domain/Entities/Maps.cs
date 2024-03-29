namespace tm20trial.Domain.Entities;

public class Maps : BaseAuditableEntity
{  
    public required string Name { get; set; }
    
    public required string Author { get; set; }

    public EDifficulty Difficulty { get; set; }
    
    public ETypeTrial TypeTrial { get; set; }

    public int Points { get; set; }

    public string? FileUrl { get; set; }
    
    public string? TmIoId { get; set; }
    
    public string? TmxLink { get; set; }

    public string? VideoLink { get; set; }
    
    public string? ImageLink { get; set; }

    public int NumberCheckpoint { get; set; }

    public EStateValidation State { get; set; } 
    
    public virtual ICollection<Records> Records { get; set; } = new HashSet<Records>();
}
