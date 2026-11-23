using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Ancient;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class WudingRiver : ShoreMagicianCardModel
{
    public WudingRiver() : base(1, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
        WithKeywords(CardKeyword.Innate);
        WithPower<WudingRiverPower>(1, 0, false);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            var basePower = Owner.Creature.GetPower<WudingRiverPower>();
            if (basePower != null)
                await PowerCmd.Remove(basePower);

            await PowerCmd.Apply<WudingRiverUpgradedPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        }
        else if (!Owner.Creature.HasPower<WudingRiverUpgradedPower>())
        {
            await PowerCmd.Apply<WudingRiverPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        }
    }
}
