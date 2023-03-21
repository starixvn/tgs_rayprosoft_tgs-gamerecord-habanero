using System.Text;
using System.Text.Json;
using Tgs.GameRecord.HabaneroService.Core.ApplicationServices;
using Tgs.MessageQueue;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.HabaneroService.Messages;

internal class ScheduleMessageHandler : IMessageHandler
{
    private readonly ILogger<ScheduleMessageHandler> _logger;
    private readonly ICommandService<SchedulePullRecordCommand> _service;

    public ScheduleMessageHandler(
        ILogger<ScheduleMessageHandler> logger,
        ICommandService<SchedulePullRecordCommand> service
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public ValueTask HandleAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
    {
        var json = Encoding.UTF8.GetString(data.Span);
        var message = JsonSerializer.Deserialize<ScheduleMessageModel>(json);

        if (message == null)
        {
            _logger.LogError("Information Received:{message}", message);
            return ValueTask.CompletedTask;
        }

        _logger.LogInformation("Information Received:{message}", message);

        var endTime = DateTimeOffset.ParseExact(DateTimeOffset.UtcNow.ToString(@"yyyy-MM-dd\THH:mm:00K"), @"yyyy-MM-dd\THH:mm:ssK", null, styles: System.Globalization.DateTimeStyles.RoundtripKind);

        var startTime = endTime.AddMinutes(-3);

        var command = new SchedulePullRecordCommand(
            ApiUrl: message.ApiUrl,
            BrandId: message.BrandId,
            ApiKey: message.ApiKey,
            StartTime: startTime,
            EndTime: endTime);

        return _service.ExecuteAsync(
            command: command,
            cancellationToken: cancellationToken);
    }
}
