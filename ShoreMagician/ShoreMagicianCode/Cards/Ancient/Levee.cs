using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Ancient;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Levee : ShoreMagicianCardModel
{
    public Levee() : base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
    {
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithVar("AdroitAmount", 4);
        WithEnchantmentTip<Adroit>(4);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var eligibleCount = Owner.GetHand().Count(c => c != this && ShoreMagicianCmd.CanEnchantWith<Adroit>(c));
        if (eligibleCount == 0)
            return;

        var chosen = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && ShoreMagicianCmd.CanEnchantWith<Adroit>(c),
            0,
            eligibleCount);

        var amount = DynamicVars["AdroitAmount"].IntValue;
        foreach (var card in chosen.ToList())
            await ShoreMagicianCmd.EnchantCard<Adroit>(choiceContext, Owner, card, amount, this);
    }
}
