using Tgs.GameRecord.Habanero.Core.JobApplicationServices;

namespace Tgs.GameRecord.Habanero.Core;

public interface IHangfireUtensil
{
    public Task JobCheckAsync();

    public Task JobExecuteAsync(JobExecuteCommand source);

    public Task JobInitAsync();
}
