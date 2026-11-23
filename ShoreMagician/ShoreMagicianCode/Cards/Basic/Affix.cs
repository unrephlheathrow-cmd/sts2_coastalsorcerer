using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Basic;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Affix : ShoreMagicianCardModel
{
    public Affix() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithVar("SharpAmount", 6, 2);
        WithTips(e => HoverTipFactory.FromEnchantment<Sharp>(e.DynamicVars["SharpAmount"].IntValue));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.Type == CardType.Attack && ShoreMagicianCmd.CanEnchantWith<Sharp>(c));

        if (chosen != null)
            await ShoreMagicianCmd.EnchantCard<Sharp>(choiceContext, Owner, chosen, DynamicVars["SharpAmount"].IntValue, this);
    }
}
