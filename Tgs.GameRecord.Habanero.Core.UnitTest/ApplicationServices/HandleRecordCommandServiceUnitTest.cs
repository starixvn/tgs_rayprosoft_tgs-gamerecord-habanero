using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.HabaneroService.Protos;
using Tgs.GameRecord.Habanero.Core.ApplicationServices;
using Tgs.GameRecord.Handle.Client;
using Tgs.GameRecord.Handle.Common.Protos;

namespace Tgs.GameRecord.Habanero.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
    [Fact]
    public async void Loss()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "HABATF"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

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
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Record.PlayerCode),
            wagerState: Arg.Is(WagerStates.Lose),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Record.ExternalRoundId),
            externalWagerId: Arg.Is(command.Record.ExternalRoundId),
            externalGameId: Arg.Is(command.Record.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Void()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "HABATF"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

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
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Record.PlayerCode),
            wagerState: Arg.Is(WagerStates.Void),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Record.ExternalRoundId),
            externalWagerId: Arg.Is(command.Record.ExternalRoundId),
            externalGameId: Arg.Is(command.Record.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Win()
    {
        var gameRecordInfo = Options.Create(new GameRecordInfoOptions
        {
            GameProviderCode = "HABATF"
        });
        var fakeClient = Substitute.For<IGameRecordHandleClient>();

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
            gameRecordInfo,
            fakeClient);

        await service.ExecuteAsync(command);

        await fakeClient
            .Received(1)
            .MessageAsync(
            gameProviderCode: Arg.Is(gameRecordInfo.Value.GameProviderCode),
            playerCode: Arg.Is(command.Record.PlayerCode),
            wagerState: Arg.Is(WagerStates.Win),
            wagerType: Arg.Is(WagerTypes.Normal),
            freeGameState: Arg.Is(FreeGameStates.None),
            gambleState: Arg.Is(GambleStates.None),
            cashoutState: Arg.Is(CashoutStates.None),
            resettleState: Arg.Is(ResettleStates.None),
            externalRoundId: Arg.Is(command.Record.ExternalRoundId),
            externalWagerId: Arg.Is(command.Record.ExternalRoundId),
            externalGameId: Arg.Is(command.Record.ExternalGameId),
            externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
            externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalPayoffOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
            externalResettleOn: Arg.Is(DateTimeOffset.UnixEpoch),
            bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
            commission: Arg.Is(0.0m),
            turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
            winLoss: Arg.Is(command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0)),
            additionalData: Arg.Is(command.Record.AdditionalData),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
