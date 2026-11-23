using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class VortexFormPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override CardLocation ModifyCardPlayResultLocation(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card.Owner?.Creature == Owner && cardLocation.pileType == PileType.Discard)
            return new CardLocation(card.Owner, PileType.Draw, CardPilePosition.Bottom);

        return cardLocation;
    }

    public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation resultLocation)
    {
        if (card.Owner?.Creature == Owner)
            Flash();

        return Task.CompletedTask;
    }
}
