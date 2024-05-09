namespace Tgs.GameRecord.Habanero.Core.JobApplicationServices;

public record JobExecuteCommand(
	string GameProviderCode,
	string Name,
	Uri ApiUrl,
	string BrandId,
	string ApiKey);
