namespace Tgs.GameRecord.HabaneroService.Core.DispatchServices;

public record ScheduleDispatchCommand(
    Uri ApiUrl,    
    string BrandId,
    string ApiKey);
