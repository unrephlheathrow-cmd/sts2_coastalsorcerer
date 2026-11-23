using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class HotSpring : ShoreMagicianCardModel
{
    public HotSpring() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(13, 4);
        WithKeywords(CardKeyword.Exhaust);
        WithTips(_ => [HoverTipFactory.FromPower<WeakPower>()]);
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var hitCount = ResolveEnergyXValue();
        var weak = Owner.Creature.GetPower<WeakPower>();
        if (weak != null)
            await PowerCmd.Remove(weak);

        if (hitCount > 0)
        {
            await CommonActions.CardAttack(this, cardPlay)
                .WithHitCount(hitCount)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }
}
