using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class DrySeaStone : ShoreMagicianCardModel
{
    public DrySeaStone() : base(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCostUpgradeBy(-1);
        WithBlock(21, 0);
        WithVar("EnchantCount", 2);
        WithEnchantmentTip<Steady>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var targetCount = DynamicVars["EnchantCount"].IntValue;
        var eligibleCount = Owner.GetHand().Count(c => c != this && ShoreMagicianCmd.CanEnchantWith<Steady>(c));
        if (eligibleCount == 0)
            return;

        var selectCount = Math.Min(targetCount, eligibleCount);
        var chosen = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && ShoreMagicianCmd.CanEnchantWith<Steady>(c),
            selectCount,
            selectCount);

        foreach (var card in chosen.ToList())
            await ShoreMagicianCmd.EnchantCard<Steady>(choiceContext, Owner, card, 1, this);
    }
}
