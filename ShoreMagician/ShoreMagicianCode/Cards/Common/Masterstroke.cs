using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Core;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Masterstroke : ShoreMagicianCardModel
{
    public Masterstroke() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(14, 5);
        WithEnergy(2);
        WithEnergyTip();
    }

    protected override bool ShouldGlowGoldInternal =>
        Owner != null && IsInCombat && ShoreMagicianModel.HasTransformedAttackThisTurn(Owner);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_heavy_blunt")
            .Execute(choiceContext);

        if (ShoreMagicianModel.HasTransformedAttackThisTurn(Owner))
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }
}
