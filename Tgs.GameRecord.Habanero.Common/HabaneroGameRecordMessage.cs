using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.Habanero.Common;

public record HabaneroGameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	[DefaultValue(typeof(Uri), "https://ws-test.insvr.com/")]
	public Uri ApiUrl { get; init; } = default!;

	[Required]
	[DefaultValue(typeof(string), "b97e1834-d142-ed11-a27c-0050f23876f7")]
	public string BrandId { get; init; } = default!;

	[Required]
	[DefaultValue(typeof(string), "0F0B9E83-80FF-4DC7-8C8C-DF48B55DAE45")]
	public string ApiKey { get; init; } = default!;

	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2024-01-04T10:00:00+08:00")]
	public DateTimeOffset EndTime { get; init; }

	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2024-01-04T11:00:00+08:00")]
	public DateTimeOffset StartTime { get; init; }
}
