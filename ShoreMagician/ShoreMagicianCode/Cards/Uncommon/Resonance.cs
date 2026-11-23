using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Resonance : ShoreMagicianCardModel
{
    public Resonance() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(5, 1);
        WithVar("SharpAmount", 2);
        WithEnchantmentTip<Sharp>(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        var chosen = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.Type == CardType.Attack && ShoreMagicianCmd.CanEnchantWith<Sharp>(c),
            0,
            3);

        var amount = DynamicVars["SharpAmount"].IntValue;
        foreach (var card in chosen.ToList())
            await ShoreMagicianCmd.EnchantCard<Sharp>(choiceContext, Owner, card, amount, this);
    }
}
