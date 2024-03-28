namespace tm20trial.Application.Common.Models;

public class NadeoMapResponse
{
    public string? Author { get; set; }
    public int AuthorScore { get; set; }
    public int BronzeScore { get; set; }
    public string? CollectionName { get; set; }
    public bool CreatedWithGamepadEditor { get; set; }
    public bool CreatedWithSimpleEditor { get; set; }
    public string? Filename { get; set; }
    public int GoldScore { get; set; }
    public bool IsPlayable { get; set; }
    public string? MapId { get; set; }
    public string? MapStyle { get; set; }
    public string? MapType { get; set; }
    public string? MapUid { get; set; }
    public string? Name { get; set; }
    public int SilverScore { get; set; }
    public string? Submitter { get; set; }
    public DateTime Timestamp { get; set; }
    public string? FileUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? AuthorDisplayName { get; set; }
}
