namespace Tgs.GameRecord.HabaneroManualService;

public record ManualMessageRequest(
    Uri ApiUrl,    
    string BrandId,
    string ApiKey,
    DateTime StartTime,
    DateTime EndTime);
