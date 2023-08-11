using Tgs.GameRounds.Work.Client;
using Tgs.GameRounds.Work.Common.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.HabaneroService.Core.ApplicationServices;

internal class HandleRecordCommandService : ICommandService<HandleRecordCommand>
{
    private readonly IUtensil _utensil;
    private readonly IGameRoundsWorkClient _workClient;

    public HandleRecordCommandService(
        IUtensil utensil,
        IGameRoundsWorkClient workClient)
    {
        _utensil = utensil ?? throw new ArgumentNullException(nameof(utensil));
        _workClient = workClient ?? throw new ArgumentNullException(nameof(workClient));
    }

    public async ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
    {
        var gameProviderId = await _utensil
            .GetGameProviderIdAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameProviderCode = _utensil.GetGameProviderCode();

        var playerModel = await _utensil
            .GetPlayerAsync(
            playerCode: command.Record.PlayerCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameId = await _utensil
            .GetGameIdAsync(
            gameProviderId: gameProviderId,
            gameExternalId: command.Record.ExternalGameId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameProviderConfigurationId = await _utensil
            .GetBrandGameProviderConfigurationIdAsync(
            brandId: playerModel.BrandId,
            gameProviderId: gameProviderId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var status = RoundStatus.Closed;
        var externalCreatedOn = command.Record.ExternalCreatedOn.ToDateTimeOffset();
        var externalClosedOn = command.Record.ExternalClosedOn.ToDateTimeOffset();
        var bet = command.Record.Bet.ToDecimal().GetValueOrDefault(0);
        var validBet = command.Record.Bet.ToDecimal().GetValueOrDefault(0);
        var settle = command.Record.Settle.ToDecimal().GetValueOrDefault(0);
        var commission = 0;
        var turnover = command.Record.Turnover.ToDecimal().GetValueOrDefault(0);
        var winLoss = command.Record.PlayerWinLoss.ToDecimal().GetValueOrDefault(0);
        var clientStatus = command.Record.GameStateId is 4
            ? RoundClientStatus.Void
            : command.Record.PlayerWinLoss < 0
            ? RoundClientStatus.Loss
            : command.Record.PlayerWinLoss > 0
            ? RoundClientStatus.Win
            : RoundClientStatus.Draw;

        await _workClient
        .RoundExchangeRateBiddingAsync(
            gameProviderId: gameProviderId,
            gameProviderCode: gameProviderCode,
            externalRoundId: command.Record.ExternalRoundId,
            gameId: gameId,
            externalGameId: command.Record.ExternalGameId,
            playerCode: playerModel.PlayerCode,
            walletId: playerModel.DefaultWalletId,
            currencyCode: playerModel.DefaultWalletCurrencyCode,
            brandId: playerModel.BrandId,
            gameProviderConfigurationId: gameProviderConfigurationId,
            status: status,
            clientStatus: clientStatus,
            externalCreatedOn: externalCreatedOn,
            externalClosedOn: externalClosedOn,
            bet: bet,
            validBet: validBet,
            settle: settle,
            commission: commission,
            turnover: turnover,
            playerWinLoss: winLoss,
            additionalData: command.Record.AdditionalData,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
    }
}
