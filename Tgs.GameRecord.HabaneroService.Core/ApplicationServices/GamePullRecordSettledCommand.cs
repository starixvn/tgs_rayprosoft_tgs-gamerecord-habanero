using Tgs.GameInterface.HabaneroService.Protos;

namespace Tgs.GameRecord.HabaneroService.Core.ApplicationServices;

public record GamePullRecordSettledCommand(IEnumerable<GameHistorySettled> Records);
