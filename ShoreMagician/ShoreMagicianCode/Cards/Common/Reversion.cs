using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Reversion : ShoreMagicianCardModel
{
    public Reversion() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithEnergy(2);
        WithEnergyTip();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var prompt = IsUpgraded
            ? new LocString("cards", $"{Id.Entry}.upgradedSelectionPrompt")
            : SelectionScreenPrompt;

        var chosen = await CommonActions.SelectSingleCard(
            this,
            prompt,
            choiceContext,
            PileType.Hand,
            c => c != this && (c.Enchantment != null || (IsUpgraded && c.IsUpgraded)));

        if (chosen == null)
            return;

        var cleared = IsUpgraded
            ? await ShoreMagicianCmd.ClearUpgradeOrEnchantment(choiceContext, Owner, chosen, this)
            : await ShoreMagicianCmd.ClearEnchantment(choiceContext, Owner, chosen, this);

        if (cleared)
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }
}
