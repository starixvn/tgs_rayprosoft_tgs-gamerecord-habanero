using System.Text;
using System.Text.Json;
using Tgs.GameRecord.Habanero.Common;
using Tgs.GameRecord.Habanero.Core;
using Tgs.GameRecord.Habanero.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.Habanero.Messages;

internal class ManualMessageHandler(
	ILogger<ManualMessageHandler> logger,
	IPullRecordDispatcher dispatcher) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var json = Encoding.UTF8.GetString(data.Span);
		var message = JsonSerializer.Deserialize<HabaneroGameRecordMessage>(json)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("{logInformation}",
			JsonSerializer.Serialize(new
			{
				LogOn = DateTime.Now.ToString("u"),
				Subject = subject,
				Data = new
				{
					message.ApiUrl,
					message.BrandId,
					message.ApiKey,
					StartTime = message.StartTime.ToString("u"),
					EndTime = message.EndTime.ToString("u")
				},
				Message = "This message receiving handler."
			}));

		return dispatcher.PushCommandAsync(
			command: new PullRecordCommand(
				ApiUrl: message.ApiUrl,
				BrandId: message.BrandId,
				ApiKey: message.ApiKey,
				StartTime: message.StartTime,
				EndTime: message.EndTime),
			cancellationToken: cancellationToken);
	}
}
