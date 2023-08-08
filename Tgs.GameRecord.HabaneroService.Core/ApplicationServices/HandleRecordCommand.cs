using Tgs.GameInterface.HabaneroService.Protos;

namespace Tgs.GameRecord.HabaneroService.Core.ApplicationServices;

public record HandleRecordCommand(GameHistorySettled GameRecord);
