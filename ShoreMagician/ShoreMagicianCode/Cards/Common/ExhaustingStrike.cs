using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ExhaustingStrike : ShoreMagicianCardModel
{
    public ExhaustingStrike() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(11, 4);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await PowerCmd.Apply<ExhaustingStrikeCostPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        var costPower = Owner.Creature.GetPower<ExhaustingStrikeCostPower>();
        costPower?.RegisterExcluded(this);
    }
}
