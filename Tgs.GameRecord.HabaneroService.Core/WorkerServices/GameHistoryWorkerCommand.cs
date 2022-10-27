namespace Tgs.GameRecord.HabaneroService.Core.WorkerServices;

public record GameHistoryWorkerCommand(
    Uri ApiUrl,
    string BrandId,
    string ApiKey,
    DateTime StartTime,
    DateTime EndTime);
