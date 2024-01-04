using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.Habanero.Common;

public record HabaneroGameRecordMessage
{
    [Required]
    public string ApiKey { get; init; } = default!;

    [Required]
    [DataType(DataType.Url)]
    public Uri ApiUrl { get; init; } = default!;

    [Required]
    public string BrandId { get; init; } = default!;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset EndTime { get; init; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset StartTime { get; init; }
}
