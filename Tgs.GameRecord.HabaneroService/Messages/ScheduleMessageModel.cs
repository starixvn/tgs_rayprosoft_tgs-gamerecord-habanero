namespace Tgs.GameRecord.HabaneroService.Messages;

public record ScheduleMessageModel(
    Uri ApiUrl,
    string BrandId,
    string ApiKey);
