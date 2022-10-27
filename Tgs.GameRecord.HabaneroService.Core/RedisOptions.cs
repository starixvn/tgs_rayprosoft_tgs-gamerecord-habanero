namespace Tgs.GameRecord.HabaneroService.Core;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}
