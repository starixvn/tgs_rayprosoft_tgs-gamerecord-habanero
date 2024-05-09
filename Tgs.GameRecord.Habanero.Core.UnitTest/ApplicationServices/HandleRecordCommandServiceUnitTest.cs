using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.HabaneroService.Protos;
using Tgs.GameRecord.Habanero.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.Habanero.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Loss()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var gameRecordInfo = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "HABATF",
			GameProviderConfiguration = "HABATFConfiguration",
			HangfirePrefix = "habatf-schedule:hangfire:",
			HangfireLocking = "habatf-locking:hangfire:",
			IntervalMinutes = 10,
			IntervalMinutesMax = 1440,
			IntervalMinutesMinimum = 1
		});
		var fakeMessageSender = Substitute.For<IMessageSender>();

		var command = new HandleRecordCommand(
			new GameHistorySettled
			{
				ExternalRoundId = "79003315691",
				ExternalGameId = "SGMountMazuma",
				PlayerCode = "4ukop6pr8zj6",
				GameStateId = 3,
				ExternalCreatedOn = DateTimeOffset.Parse("2023-08-10T06:26:04.49").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-08-10T06:26:04.59").ToTimestamp(),
				Bet = 1.4m,
				ValidBet = 1.4m,
				Settle = 0,
				Turnover = 1.4m,
				PlayerWinLoss = -1.4m,
				AdditionalData = "{\"GameStateId\":3,\"FeatureCount\":0,\"BuyFeatureId\":0,\"PlayerId\":\"89dc452b-4637-ee11-b8f0-6045bd569110\",\"BrandId\":\"a452e523-8d40-ed11-a27c-00155db8f23b\",\"Username\":\"4ukop6pr8zj6\",\"BrandGameId\":\"01acc930-4978-4d9d-9b7d-7e961f6894ec\",\"GameKeyName\":\"SGMountMazuma\",\"GameTypeId\":11,\"DtStarted\":\"2023-08-10T06:26:04.49\",\"DtCompleted\":\"2023-08-10T06:26:04.59\",\"FriendlyGameInstanceId\":79003315691,\"GameInstanceId\":\"a2cb7cc6-4637-ee11-b8f0-6045bd569110\",\"Stake\":1.4,\"Payout\":0.0,\"JackpotWin\":0.0,\"JackpotContribution\":0.0,\"CurrencyCode\":\"HKD\",\"ChannelTypeId\":4,\"BalanceAfter\":88.8}"
			});

		var service = new HandleRecordCommandService(
			faceLogger,
			gameRecordInfo,
			fakeMessageSender);

		await service.ExecuteAsync(command);

		_ = fakeMessageSender
			.Received(1)
			.PublishAsync(
			subject: Arg.Is(Handle.Common.GameRecordHandleNatsConst.GameRecordHandleRequest),
			data: Arg.Is(new Handle.Common.Protos.GameRecordHandleRequest
			{
				GameProviderCode = gameRecordInfo.Value.GameProviderCode,
				PlayerCode = command.Record.PlayerCode,
				ExternalRoundId = command.Record.ExternalRoundId,
				ExternalBetId = command.Record.ExternalRoundId,
				ExternalGameId = command.Record.ExternalGameId,
				ExternalCreatedOn = command.Record.ExternalCreatedOn,
				ExternalClosedOn = command.Record.ExternalClosedOn,
				ExternalPayoffOn = command.Record.ExternalClosedOn,
				ExternalResettleOn = DateTimeOffset.UnixEpoch.ToTimestamp(),
				BetState = Handle.Common.Protos.BetState.Loss,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 1.4m,
				ValidBet = 1.4m,
				Settle = 0.0M,
				Commission = 0.0M,
				Turnover = 1.4m,
				WinLoss = -1.4m,
				Jackpot = 0.0M,
				AdditionalData = command.Record.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Void()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var gameRecordInfo = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "HABATF",
			GameProviderConfiguration = "HABATFConfiguration",
			HangfirePrefix = "habatf-schedule:hangfire:",
			HangfireLocking = "habatf-locking:hangfire:",
			IntervalMinutes = 10,
			IntervalMinutesMax = 1440,
			IntervalMinutesMinimum = 1
		});
		var fakeMessageSender = Substitute.For<IMessageSender>();

		var command = new HandleRecordCommand(
			new GameHistorySettled
			{
				ExternalRoundId = "79003316977",
				ExternalGameId = "SGMountMazuma",
				PlayerCode = "4ukop6pr8zj6",
				GameStateId = 4,
				ExternalCreatedOn = DateTimeOffset.Parse("2023-08-10T06:26:08.01").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-08-10T06:26:08.037").ToTimestamp(),
				Bet = 0m,
				ValidBet = 0m,
				Settle = 0m,
				Turnover = 0m,
				PlayerWinLoss = 0m,
				AdditionalData = "{\"GameStateId\":4,\"FeatureCount\":0,\"BuyFeatureId\":0,\"PlayerId\":\"89dc452b-4637-ee11-b8f0-6045bd569110\",\"BrandId\":\"a452e523-8d40-ed11-a27c-00155db8f23b\",\"Username\":\"4ukop6pr8zj6\",\"BrandGameId\":\"01acc930-4978-4d9d-9b7d-7e961f6894ec\",\"GameKeyName\":\"SGMountMazuma\",\"GameTypeId\":11,\"DtStarted\":\"2023-08-10T06:26:08.01\",\"DtCompleted\":\"2023-08-10T06:26:08.037\",\"FriendlyGameInstanceId\":79003316977,\"GameInstanceId\":\"fcd07cc6-4637-ee11-b8f0-6045bd569110\",\"Stake\":0,\"Payout\":0,\"JackpotWin\":0.0,\"JackpotContribution\":0.0,\"CurrencyCode\":\"HKD\",\"ChannelTypeId\":4,\"BalanceAfter\":100.0}"
			});

		var service = new HandleRecordCommandService(
			faceLogger,
			gameRecordInfo,
			fakeMessageSender);

		await service.ExecuteAsync(command);

		_ = fakeMessageSender
			.Received(1)
			.PublishAsync(
			subject: Arg.Is(Handle.Common.GameRecordHandleNatsConst.GameRecordHandleRequest),
			data: Arg.Is(new Handle.Common.Protos.GameRecordHandleRequest
			{
				GameProviderCode = gameRecordInfo.Value.GameProviderCode,
				PlayerCode = command.Record.PlayerCode,
				ExternalRoundId = command.Record.ExternalRoundId,
				ExternalBetId = command.Record.ExternalRoundId,
				ExternalGameId = command.Record.ExternalGameId,
				ExternalCreatedOn = command.Record.ExternalCreatedOn,
				ExternalClosedOn = command.Record.ExternalClosedOn,
				ExternalPayoffOn = command.Record.ExternalClosedOn,
				ExternalResettleOn = DateTimeOffset.UnixEpoch.ToTimestamp(),
				BetState = Handle.Common.Protos.BetState.Void,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 0.0m,
				ValidBet = 0.0m,
				Settle = 0.0M,
				Commission = 0.0m,
				Turnover = 0.0m,
				WinLoss = 0.0m,
				Jackpot = 0.0M,
				AdditionalData = command.Record.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Win()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var gameRecordInfo = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "HABATF",
			GameProviderConfiguration = "HABATFConfiguration",
			HangfirePrefix = "habatf-schedule:hangfire:",
			HangfireLocking = "habatf-locking:hangfire:",
			IntervalMinutes = 10,
			IntervalMinutesMax = 1440,
			IntervalMinutesMinimum = 1
		});
		var fakeMessageSender = Substitute.For<IMessageSender>();

		var command = new HandleRecordCommand(
			new GameHistorySettled
			{
				ExternalRoundId = "79003316977",
				ExternalGameId = "SGMountMazuma",
				PlayerCode = "4ukop6pr8zj6",
				GameStateId = 3,
				ExternalCreatedOn = DateTimeOffset.Parse("2023-08-10T06:26:08.01").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2023-08-10T06:26:08.037").ToTimestamp(),
				Bet = 1.4m,
				ValidBet = 1.4m,
				Settle = 14.0m,
				Turnover = 1.4m,
				PlayerWinLoss = 12.6m,
				AdditionalData = "{\"GameStateId\":3,\"FeatureCount\":0,\"BuyFeatureId\":0,\"PlayerId\":\"89dc452b-4637-ee11-b8f0-6045bd569110\",\"BrandId\":\"a452e523-8d40-ed11-a27c-00155db8f23b\",\"Username\":\"4ukop6pr8zj6\",\"BrandGameId\":\"01acc930-4978-4d9d-9b7d-7e961f6894ec\",\"GameKeyName\":\"SGMountMazuma\",\"GameTypeId\":11,\"DtStarted\":\"2023-08-10T06:26:08.01\",\"DtCompleted\":\"2023-08-10T06:26:08.037\",\"FriendlyGameInstanceId\":79003316977,\"GameInstanceId\":\"fcd07cc6-4637-ee11-b8f0-6045bd569110\",\"Stake\":1.4,\"Payout\":14.0,\"JackpotWin\":0.0,\"JackpotContribution\":0.0,\"CurrencyCode\":\"HKD\",\"ChannelTypeId\":4,\"BalanceAfter\":100.0}"
			});

		var service = new HandleRecordCommandService(
			faceLogger,
			gameRecordInfo,
			fakeMessageSender);

		await service.ExecuteAsync(command);

		_ = fakeMessageSender
			.Received(1)
			.PublishAsync(
			subject: Arg.Is(Handle.Common.GameRecordHandleNatsConst.GameRecordHandleRequest),
			data: Arg.Is(new Handle.Common.Protos.GameRecordHandleRequest
			{
				GameProviderCode = gameRecordInfo.Value.GameProviderCode,
				PlayerCode = command.Record.PlayerCode,
				ExternalRoundId = command.Record.ExternalRoundId,
				ExternalBetId = command.Record.ExternalRoundId,
				ExternalGameId = command.Record.ExternalGameId,
				ExternalCreatedOn = command.Record.ExternalCreatedOn,
				ExternalClosedOn = command.Record.ExternalClosedOn,
				ExternalPayoffOn = command.Record.ExternalClosedOn,
				ExternalResettleOn = DateTimeOffset.UnixEpoch.ToTimestamp(),
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 1.4m,
				ValidBet = 1.4m,
				Settle = 14.0m,
				Commission = 0.0m,
				Turnover = 1.4m,
				WinLoss = 12.6m,
				Jackpot = 0.0M,
				AdditionalData = command.Record.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
