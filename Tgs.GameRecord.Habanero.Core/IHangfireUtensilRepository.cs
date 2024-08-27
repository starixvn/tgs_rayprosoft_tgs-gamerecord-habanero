using Tgs.GameRecord.Habanero.Core.JobApplicationServices;

namespace Tgs.GameRecord.Habanero.Core;

public interface IHangfireUtensilRepository
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}
