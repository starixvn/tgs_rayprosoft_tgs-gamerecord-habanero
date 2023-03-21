using Tgs.GameInterface.HabaneroService.Protos;

namespace Tgs.GameRecord.HabaneroService.Core.ApplicationServices;

public record ManualHandleRecordCommand(GameHistorySettled GameRecord);
