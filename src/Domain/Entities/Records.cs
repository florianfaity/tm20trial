using System.ComponentModel.DataAnnotations.Schema;

namespace tm20trial.Domain.Entities;

public class Records : BaseAuditableEntity
{
    public bool IsValidated { get; set; }

    public TimeSpan Time { get; set; }

    public int IdMap { get; set; }

    public int IdUser { get; set; }

    [ForeignKey("IdUser")]
    public virtual Users User { get; set; }
}
