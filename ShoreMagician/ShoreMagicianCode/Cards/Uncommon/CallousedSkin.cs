using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class CallousedSkin : ShoreMagicianCardModel
{
    public CallousedSkin() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        WithVar("MagicGain", 1);
        WithTips(e => e.IsUpgraded
            ? [HoverTipFactory.FromPower<TempMagicPower>()]
            : [HoverTipFactory.FromCard<SeaBreeze>(), HoverTipFactory.FromPower<TempMagicPower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var amount = DynamicVars["MagicGain"].IntValue;
        if (IsUpgraded)
            await PowerCmd.Apply<CallousedSkinUpgradedPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
        else
            await PowerCmd.Apply<CallousedSkinPower>(choiceContext, Owner.Creature, amount, Owner.Creature, this);
    }
}
