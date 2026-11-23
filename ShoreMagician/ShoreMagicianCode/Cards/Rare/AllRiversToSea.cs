using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class AllRiversToSea : ShoreMagicianCardModel
{
    public AllRiversToSea() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithTips(e => e.IsUpgraded ? [HoverTipFactory.FromKeyword(CardKeyword.Ethereal)] : []);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            var basePower = Owner.Creature.GetPower<AllRiversToSeaPower>();
            if (basePower != null)
                await PowerCmd.Remove(basePower);

            await PowerCmd.Apply<AllRiversToSeaUpgradedPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        }
        else if (!Owner.Creature.HasPower<AllRiversToSeaUpgradedPower>())
        {
            await PowerCmd.Apply<AllRiversToSeaPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        }
    }
}
