namespace Tgs.GameRecord.HabaneroManualService.ViewModels;

public record GameRecordQueryViewModel(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    DateTime StartTime,
    DateTime EndTime);
