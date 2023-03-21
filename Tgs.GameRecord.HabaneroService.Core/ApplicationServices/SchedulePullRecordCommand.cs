namespace Tgs.GameRecord.HabaneroService.Core.ApplicationServices;

public record SchedulePullRecordCommand(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
