using Microsoft.Extensions.Logging;
using Tgs.GameInterface.HabaneroService.Client;
using Tgs.GameInterface.HabaneroService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.Habanero.Core.ApplicationServices;

internal class PullRecordCommandService(
	ILogger<PullRecordCommandService> logger,
	IGameInterfaceHabaneroClient client,
	ICommandService<HandleRecordCommand> service) : ICommandService<PullRecordCommand>
{
	public async ValueTask ExecuteAsync(PullRecordCommand command, CancellationToken cancellationToken = default)
	{
		var gameRecords = await client.GameHistorySettledAsync(
			apiUrl: command.ApiUrl,
			brandId: command.BrandId,
			apiKey: command.ApiKey,
			startTime: command.StartTime.UtcDateTime,
			endTime: command.EndTime.UtcDateTime,
			cancellationToken: cancellationToken)
			.ToArrayAsync(cancellationToken: cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameHistorySettled>();

		logger.LogInformation("LogOn:{logOn} GameRecords:{gameRecords}", DateTime.Now.ToString("o"), gameRecords);

		if (!gameRecords.Any())
		{
			return;
		}

		foreach (var gameRecord in gameRecords)
		{
			try
			{
				await service.ExecuteAsync(
					command: new(gameRecord),
					cancellationToken: cancellationToken)
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "LogOn:{logOn} Message: An exception occurred in this game record.({gameRecord})", DateTime.Now.ToString("o"), gameRecord);
			}
		}
	}
}
