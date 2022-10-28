using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.HabaneroManualService.ViewModels;
using Tgs.GameRecord.HabaneroService.Core.WorkerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.HabaneroManualService.Controllers;

[ApiController]
[Route("[controller]")]
public class HabaneroController : ControllerBase
{
    /// <summary>
    /// Games the history by sports book asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com/",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "startTime": "2022-10-26T17:10:00+08:00",
    ///             "endTime": "2022-10-26T17:25:00+08:00
    ///         }
    /// </remarks>
    [HttpPost]
    public ValueTask GameHistoryAsync(
            [FromServices] ICommandService<GameHistoryWorkerCommand> service,
            GameRecordQueryViewModel model)
            => service.ExecuteAsync(new GameHistoryWorkerCommand(
                ApiUrl: model.ApiUrl,
                BrandId: model.BrandId,
                ApiKey: model.ApiKey,
                StartTime: model.StartTime,
                EndTime: model.EndTime));
}
