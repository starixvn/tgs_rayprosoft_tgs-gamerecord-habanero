using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.HabaneroService.Client;
using Tgs.GameRecord.HabaneroService.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.HabaneroService.Core.WorkerServices;

internal class GameHistoryWorkerService : ICommandService<GameHistoryWorkerCommand>
{
    private readonly IGameInterfaceHabaneroClient _client;
    private readonly ILogger<GameHistoryWorkerService> _logger;
    private readonly ICommandService<GamePullRecordSettledCommand> _settledService;

    public GameHistoryWorkerService(
        IGameInterfaceHabaneroClient client,
        ILogger<GameHistoryWorkerService> logger,
        ICommandService<GamePullRecordSettledCommand> settledService)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settledService = settledService ?? throw new ArgumentNullException(nameof(settledService));
    }

    public async ValueTask ExecuteAsync(GameHistoryWorkerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            while (true)
            {
                var gameHistorySettleds = await _client
                        .GameHistorySettledAsync(
                            apiUrl: command.ApiUrl,
                            brandId: command.BrandId,
                            apiKey: command.ApiKey,
                            startTime: command.StartTime.ToUniversalTime(),
                            endTime: command.EndTime.ToUniversalTime(),
                            cancellationToken: cancellationToken)
                        .ToArrayAsync(cancellationToken: cancellationToken)
                        .ConfigureAwait(false);

                _logger.LogInformation("LogOn:{logOn} | Action:GameHistorySettledAsync | Response:{response}",
                    DateTime.UtcNow.ToString("O"),
                    JsonSerializer.Serialize(gameHistorySettleds));

                if (!gameHistorySettleds.Any())
                {
                    break;
                }

                await _settledService
                    .ExecuteAsync(
                        command: new GamePullRecordSettledCommand(
                            Records: gameHistorySettleds),
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LogOn:{logOn} | Action:GameHistorySettledAsync | Message:Habanero處理結算過程中發生例外", DateTime.UtcNow.ToString("O"));
        }
    }
}
