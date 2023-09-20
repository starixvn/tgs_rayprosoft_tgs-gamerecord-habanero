using Hangfire.Dashboard;

namespace Tgs.GameRecord.Habanero;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}
