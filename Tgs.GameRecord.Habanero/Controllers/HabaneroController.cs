using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.Habanero.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.Habanero.Controllers;

[Route("HABATF")]
[ApiController]
public class HabaneroController : ControllerBase
{
    /// <summary>
    /// Games the record manual asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com/",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "startTime": "2024-01-04T10:00:00+08:00",
    ///             "endTime": "2024-01-04T11:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("manual")]
    public ValueTask GameRecordManualAsync(
        [FromServices] IMessageSender messageSender,
        HabaneroGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: HabaneroNatsConst.ManualPublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the record schedule asynchronous.
    /// </summary>
    /// <param name="messageSender">The message sender.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "apiUrl": "https://ws-test.insvr.com/",
    ///             "brandId": "b97e1834-d142-ed11-a27c-0050f23876f7",
    ///             "apiKey": "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45",
    ///             "startTime": "2024-01-04T10:00:00+08:00",
    ///             "endTime": "2024-01-04T11:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("schedule")]
    public ValueTask GameRecordScheduleAsync(
        [FromServices] IMessageSender messageSender,
        HabaneroGameRecordMessage source)
        => messageSender.PublishAsync(
            subject: HabaneroNatsConst.SchedulePublish,
            data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
            cancellationToken: HttpContext.RequestAborted);
}
