using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ArcaneGuidance : ShoreMagicianCardModel
{
    public ArcaneGuidance() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(7, 3);
        WithTips(_ => [HoverTipFactory.FromPower<VulnerablePower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if (cardPlay.Target == null)
            return;

        var enchantedInHand = Owner.GetHand().Count(c => c.Enchantment != null);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, enchantedInHand + 1, Owner.Creature, this);
    }
}
