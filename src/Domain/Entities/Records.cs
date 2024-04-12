using System.ComponentModel.DataAnnotations.Schema;

namespace tm20trial.Domain.Entities;

public class Records : BaseAuditableEntity
{      
    public bool IsValidated { get; set; }

    public TimeSpan Time { get; set; }
    
    public DateTime DatePersonalBest { get; set; }

    public int IdMap { get; set; }

    public int IdUser { get; set; }
    
    public string? FileUrl { get; set; }
    
    public EMedal Medal { get; set; }

    [ForeignKey("IdUser")]
    public virtual Users? User { get; set; }
    
    [ForeignKey("IdMap")]
    public virtual Maps? Map { get; set; }
}
