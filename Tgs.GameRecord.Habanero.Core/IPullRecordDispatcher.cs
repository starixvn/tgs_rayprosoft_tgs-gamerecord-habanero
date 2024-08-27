using Tgs.GameRecord.Habanero.Core.ApplicationServices;

namespace Tgs.GameRecord.Habanero.Core;

public interface IPullRecordDispatcher
{
	ValueTask PushCommandAsync(PullRecordCommand command, CancellationToken cancellationToken = default);
}
