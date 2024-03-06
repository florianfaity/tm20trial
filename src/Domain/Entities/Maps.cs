namespace tm20trial.Domain.Entities;

public class Maps : BaseAuditableEntity
{  
    public required string Name { get; set; }
    
    public required string Author { get; set; }

    public EDifficulty Difficulty { get; set; }
    
    public ETypeTrial TypeTrial { get; set; }

    public int Points { get; set; }

    public string? TmxLink { get; set; }

    public string? VideoLink { get; set; }
    
    public string? ImageLink { get; set; }

    public int NumberCheckpoint { get; set; }

    public bool Validate { get; set; } = false;
    
    public virtual ICollection<Records> Records { get; set; } = new HashSet<Records>();
}
