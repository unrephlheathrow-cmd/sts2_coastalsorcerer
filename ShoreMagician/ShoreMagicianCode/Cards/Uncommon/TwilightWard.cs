using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class TwilightWard : ShoreMagicianCardModel
{
    public TwilightWard() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithBlock(16, 2);
        WithPower<WeakPower>(2, 0, true);
        WithTips(_ => [HoverTipFactory.FromPower<SlowPower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var attackingOpponents = Owner.GetAllOpponents()
            .Where(e => e.Monster?.IntendsToAttack == true)
            .ToList();

        if (attackingOpponents.Count == 0)
            return;

        await PowerCmd.Apply<WeakPower>(choiceContext, attackingOpponents, DynamicVars["WeakPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<SlowPower>(choiceContext, attackingOpponents, 1m, Owner.Creature, this);
    }
}
