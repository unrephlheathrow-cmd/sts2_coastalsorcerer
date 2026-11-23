using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Core;
using ShoreMagician.Code.Extensions;

using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ManaStrike : ShoreMagicianCardModel
{
    public ManaStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(10, 3);
        WithVar("BonusDamage", 4);
    }

    public bool ShouldCountAsStrike =>
        Enchantment != null ||
        (Owner != null && ShoreMagicianModel.HasPlayedOverflow(Owner));

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (cardSource != this || !props.IsPoweredAttack() || Owner == null || !IsInCombat)
            return 0m;

        var enchantedCount = Owner.GetHand().Count(c => c != this && c.Enchantment != null);
        return enchantedCount * DynamicVars["BonusDamage"].BaseValue;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
}
