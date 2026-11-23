using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Awaken : ShoreMagicianCardModel
{
    public Awaken() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(4, 1);
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var unenchanted = Owner.GetHand().Where(c => c != this && c.Enchantment == null).ToList();
        if (unenchanted.Count == 0)
            return;

        IReadOnlyList<CardModel> toExhaust;
        if (IsUpgraded)
        {
            toExhaust = (await CommonActions.SelectCards(
                this,
                SelectionScreenPrompt,
                choiceContext,
                PileType.Hand,
                c => c != this && c.Enchantment == null,
                0,
                unenchanted.Count)).ToList();
        }
        else
        {
            toExhaust = unenchanted;
        }

        foreach (var card in toExhaust)
        {
            await CardCmd.Exhaust(choiceContext, card);
            await CommonActions.CardBlock(this, cardPlay);
        }
    }
}
