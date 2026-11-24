using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class SublimationExtract : ShoreMagicianCardModel
{
    public SublimationExtract() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(10, 0);
        WithPower<FreeEnchantedCardsPower>(1, 1, false);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && (c.Enchantment != null || c.IsUpgraded));

        if (chosen != null && await ShoreMagicianCmd.ClearUpgradeOrEnchantment(choiceContext, Owner, chosen, this))
        {
            await CommonActions.ApplySelf<FreeEnchantedCardsPower>(choiceContext, this);
            var power = Owner.Creature.GetPower<FreeEnchantedCardsPower>();
            if (power != null)
                power.IgnoreCurrentPlayCard = this;
        }
    }
}
