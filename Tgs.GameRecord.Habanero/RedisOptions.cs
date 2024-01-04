using Tgs.GameRecord.Habanero.Core;
namespace Tgs.GameRecord.Habanero;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}
