namespace Tgs.GameRecord.HabaneroService.Core.ApplicationServices;

public record ManualPullRecordCommand(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
