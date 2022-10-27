namespace Tgs.GameRecord.HabaneroManualService;

public record NatsSettingOptions
{
    public string Queue { get; set; } = default!;

    public string Subject { get; set; } = default!;
}
