namespace Tgs.GameRecord.HabaneroService.Core.ApplicationServices;

public record PullRecordCommand(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
