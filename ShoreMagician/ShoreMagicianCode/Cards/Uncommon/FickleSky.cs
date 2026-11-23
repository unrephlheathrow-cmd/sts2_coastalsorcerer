using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class FickleSky : ShoreMagicianCardModel
{
    public FickleSky() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(13, 2);
        WithVar("EnchantCount", 2);
        WithEnchantmentTip<Sown>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var maxCount = DynamicVars["EnchantCount"].IntValue;
        var chosen = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Discard,
            ShoreMagicianCmd.CanReplaceEnchantmentWith<Sown>,
            0,
            maxCount);

        foreach (var card in chosen.ToList())
            await ShoreMagicianCmd.ReplaceEnchantment<Sown>(choiceContext, Owner, card, 1, this);
    }
}
