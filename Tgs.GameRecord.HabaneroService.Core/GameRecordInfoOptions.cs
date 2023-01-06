namespace Tgs.GameRecord.HabaneroService.Core;

public record GameRecordInfoOptions
{
    public string GameProviderCode { get; set; } = default!;

    public double MinimumIntervalMinute { get; set; } = 1d;

    public double IntervalMinute { get; set; } = 3d;
    public double LockMinute { get; set; } = 2d;
}
