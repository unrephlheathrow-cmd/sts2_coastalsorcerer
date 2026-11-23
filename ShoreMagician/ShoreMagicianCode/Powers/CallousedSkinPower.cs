using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;

namespace ShoreMagician.Code.Powers;

public sealed class CallousedSkinPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<SeaBreeze>(), HoverTipFactory.FromPower<TempMagicPower>()];

    public override decimal ModifyHpLostBeforeOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner && cardSource is SeaBreeze)
            return 0m;
        return amount;
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner?.Creature != Owner)
            return;
        if (card.Type is not (CardType.Curse or CardType.Status))
            return;

        Flash();
        await PowerCmd.Apply<TempMagicPower>(choiceContext, Owner, Amount, Owner, null);
    }
}

public sealed class CallousedSkinUpgradedPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<TempMagicPower>()];

    // Mirrors TungstenRod HP-loss reduction logic.
    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || amount <= 0m)
            return amount;

        return Math.Max(0m, amount - Amount);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner?.Creature != Owner)
            return;
        if (card.Type is not (CardType.Curse or CardType.Status))
            return;

        Flash();
        await PowerCmd.Apply<TempMagicPower>(choiceContext, Owner, Amount, Owner, null);
    }
}
