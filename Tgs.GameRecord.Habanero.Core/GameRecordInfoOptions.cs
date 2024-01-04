namespace Tgs.GameRecord.Habanero.Core;

public record GameRecordInfoOptions
{
    public string GameProviderCode { get; init; } = default!;

    public string GameProviderConfiguration { get; init; } = default!;

    public string HangfirePrefix { get; set; } = "gamerecord-habatf-hangfire:";
    public string HangfireLocking { get; set; } = "gamerecord-habatf-locking:";

    public double IntervalMinutes { get; init; } = 3;

    public double IntervalMinutesMax { get; init; } = 60;

    public double IntervalMinutesMinimum { get; init; } = 1;
}
