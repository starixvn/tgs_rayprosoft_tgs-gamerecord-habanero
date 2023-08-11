using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using NSubstitute;
using Tgs.GameCommand.Common;
using Tgs.GameInterface.HabaneroService.Protos;
using Tgs.GameRecord.HabaneroService.Core.ApplicationServices;
using Tgs.GameRounds.Work.Client;
using Tgs.GameRounds.Work.Common.Protos;

namespace Tgs.GameRecord.HabaneroService.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
    [Fact]
    public async void Loss()
    {
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeClient = Substitute.For<IGameRoundsWorkClient>();

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

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "FACHAI";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "PB2",
            PlayerCode: command.Record.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(command.Record.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(command.Record.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var service = new HandleRecordCommandService(
            fakeUtensil,
            fakeClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .RoundExchangeRateBiddingAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameProviderCode: Arg.Is(gameProviderCode),
                externalRoundId: Arg.Is(command.Record.ExternalRoundId),
                gameId: Arg.Is(gameId),
                externalGameId: Arg.Is(command.Record.ExternalGameId),
                playerCode: Arg.Is(player.PlayerCode),
                walletId: Arg.Is(player.DefaultWalletId),
                currencyCode: Arg.Is(player.DefaultWalletCurrencyCode),
                brandId: Arg.Is(player.BrandId),
                gameProviderConfigurationId: Arg.Is(gameProviderConfigurationId),
                status: Arg.Is(RoundStatus.Closed),
                clientStatus: Arg.Is(RoundClientStatus.Loss),
                externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
                bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
                commission: Arg.Is(0.0m),
                turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                playerWinLoss: Arg.Is(command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0)),
                additionalData: Arg.Is(command.Record.AdditionalData),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [Fact]
    public async void Win()
    {
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeClient = Substitute.For<IGameRoundsWorkClient>();

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

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "FACHAI";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "PB2",
            PlayerCode: command.Record.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(command.Record.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(command.Record.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var service = new HandleRecordCommandService(
            fakeUtensil,
            fakeClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .RoundExchangeRateBiddingAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameProviderCode: Arg.Is(gameProviderCode),
                externalRoundId: Arg.Is(command.Record.ExternalRoundId),
                gameId: Arg.Is(gameId),
                externalGameId: Arg.Is(command.Record.ExternalGameId),
                playerCode: Arg.Is(player.PlayerCode),
                walletId: Arg.Is(player.DefaultWalletId),
                currencyCode: Arg.Is(player.DefaultWalletCurrencyCode),
                brandId: Arg.Is(player.BrandId),
                gameProviderConfigurationId: Arg.Is(gameProviderConfigurationId),
                status: Arg.Is(RoundStatus.Closed),
                clientStatus: Arg.Is(RoundClientStatus.Win),
                externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
                bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
                commission: Arg.Is(0.0m),
                turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                playerWinLoss: Arg.Is(command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0)),
                additionalData: Arg.Is(command.Record.AdditionalData),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [Fact]
    public async void Void()
    {
        var fakeUtensil = Substitute.For<IUtensil>();
        var fakeClient = Substitute.For<IGameRoundsWorkClient>();

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

        var gameProviderId = Guid.NewGuid();
        _ = fakeUtensil
            .GetGameProviderIdAsync(
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderId);

        var gameProviderCode = "FACHAI";
        _ = fakeUtensil
            .GetGameProviderCode()
            .Returns(gameProviderCode);

        var player = new PlayerInfo(
            ResellerId: Guid.NewGuid(),
            ResellerCode: "TGS",
            LicenseeId: Guid.NewGuid(),
            LicenseeCode: "TGS",
            BrandId: Guid.NewGuid(),
            BrandCode: "PB2",
            PlayerCode: command.Record.PlayerCode,
            Username: "test",
            DefaultWalletId: Guid.NewGuid(),
            DefaultWalletCurrencyCode: "PHP"
            );
        _ = fakeUtensil
            .GetPlayerAsync(
                playerCode: Arg.Is(command.Record.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(player);

        var gameId = 1;
        _ = fakeUtensil
            .GetGameIdAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameExternalId: Arg.Is(command.Record.ExternalGameId.ToString()),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameId);

        var gameProviderConfigurationId = Guid.NewGuid();
        _ = fakeUtensil
            .GetBrandGameProviderConfigurationIdAsync(
                brandId: Arg.Is(player.BrandId),
                gameProviderId: Arg.Is(gameProviderId),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(gameProviderConfigurationId);

        var service = new HandleRecordCommandService(
            fakeUtensil,
            fakeClient);

        await service
            .ExecuteAsync(
                command)
            .ConfigureAwait(false);

        await fakeClient
            .Received(1)
            .RoundExchangeRateBiddingAsync(
                gameProviderId: Arg.Is(gameProviderId),
                gameProviderCode: Arg.Is(gameProviderCode),
                externalRoundId: Arg.Is(command.Record.ExternalRoundId),
                gameId: Arg.Is(gameId),
                externalGameId: Arg.Is(command.Record.ExternalGameId),
                playerCode: Arg.Is(player.PlayerCode),
                walletId: Arg.Is(player.DefaultWalletId),
                currencyCode: Arg.Is(player.DefaultWalletCurrencyCode),
                brandId: Arg.Is(player.BrandId),
                gameProviderConfigurationId: Arg.Is(gameProviderConfigurationId),
                status: Arg.Is(RoundStatus.Closed),
                clientStatus: Arg.Is(RoundClientStatus.Void),
                externalCreatedOn: Arg.Is(command.Record.ExternalCreatedOn.ToDateTimeOffset()),
                externalClosedOn: Arg.Is(command.Record.ExternalClosedOn.ToDateTimeOffset()),
                bet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                validBet: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                settle: Arg.Is(command.Record.Settle.ToDecimal().GetValueOrDefault(0)),
                commission: Arg.Is(0.0m),
                turnover: Arg.Is(command.Record.Bet.ToDecimal().GetValueOrDefault(0)),
                playerWinLoss: Arg.Is(command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0)),
                additionalData: Arg.Is(command.Record.AdditionalData),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
