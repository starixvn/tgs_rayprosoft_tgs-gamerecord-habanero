namespace Tgs.GameRecord.HabaneroService.Core;

public record GameRecordInfoOptions
{
    public string GameProviderCode { get; set; } = default!;

    public double MinIntervalMinute { get; set; } = 1d;

    public double IntervalMinute { get; set; } = 3d;
    public double LockMinute { get; set; } = 2d;

    public string MongoDefine { get; set; } = default!;

    public string MongoGlobPattern { get; set; } = default!;

    public string GameExternalId { get; set; } = default!;
}
