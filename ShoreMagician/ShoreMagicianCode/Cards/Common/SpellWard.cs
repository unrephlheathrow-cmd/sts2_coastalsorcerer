using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class SpellWard : ShoreMagicianCardModel
{
    public SpellWard() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(7, 2);
        WithVar("NimbleAmount", 3);
        WithEnchantmentTip<Nimble>(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.Type == CardType.Skill && ShoreMagicianCmd.CanEnchantWith<Nimble>(c));

        if (chosen != null)
            await ShoreMagicianCmd.EnchantCard<Nimble>(choiceContext, Owner, chosen, DynamicVars["NimbleAmount"].IntValue, this);
    }
}
