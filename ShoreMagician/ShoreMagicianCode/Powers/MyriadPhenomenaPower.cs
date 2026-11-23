using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class MyriadPhenomenaPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyHandDraw(Player player, decimal count) =>
        player == Owner.Player ? count + Amount : count;

    // Matches vanilla NostalgiaPower: checks CombatHistory.CardPlaysStarted (count == 0 for the 1st card played this turn)
    // without mutating state inside the query hook.
    public override CardLocation ModifyCardPlayResultLocation(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card.Owner?.Creature != Owner || cardLocation.pileType != PileType.Discard || CombatState == null)
            return cardLocation;

        var cardsStartedThisTurn = CombatManager.Instance.History.CardPlaysStarted
            .Count(e => e.HappenedThisTurn(CombatState) && e.CardPlay.Player == Owner.Player);

        if (cardsStartedThisTurn >= 1)
            return cardLocation;

        return new CardLocation(card.Owner, PileType.Draw, CardPilePosition.Bottom);
    }

    public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation resultLocation)
    {
        if (card.Owner?.Creature == Owner)
            Flash();

        return Task.CompletedTask;
    }
}
