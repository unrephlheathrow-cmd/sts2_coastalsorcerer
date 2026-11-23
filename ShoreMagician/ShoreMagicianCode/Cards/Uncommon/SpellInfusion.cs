using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class SpellInfusion : ShoreMagicianCardModel
{
    public SpellInfusion() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithVar("Multiplier", 4);
        WithEnchantmentTip<Sharp>(4);
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var effX = ResolveEnergyXValue() + (IsUpgraded ? 1 : 0);
        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.Type == CardType.Attack && (effX == 0 ? c.Enchantment != null : ShoreMagicianCmd.CanReplaceEnchantmentWith<Sharp>(c)));

        if (chosen == null)
            return;

        if (effX > 0)
            await ShoreMagicianCmd.ReplaceEnchantment<Sharp>(choiceContext, Owner, chosen, effX * DynamicVars["Multiplier"].IntValue, this);
        else if (chosen.Enchantment != null)
            await ShoreMagicianCmd.ClearEnchantment(choiceContext, Owner, chosen, this);
    }
}
