namespace tm20trial.Application.Common.Models;

public class RecordScore
{
    public uint RespawnCount { get; set; }
    public int Score { get; set; }
    public int Time { get; set; }
}

public class NadeoRecordResponse
{
    public string? AccountId { get; set; }
    public string? Filename { get; set; }
    public string? GameMode { get; set; }
    public string? GameModeCustomData { get; set; }
    public string? MapId { get; set; }
    public string? MapRecordId { get; set; }
    public int Medal { get; set; }
    public RecordScore? RecordScore { get; set; }
    public bool Removed { get; set; }
    public string? ScopeId { get; set; }
    public string? ScopeType { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Url { get; set; }
}
