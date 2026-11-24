using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Dredge : ShoreMagicianCardModel
{
    public Dredge() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithCards(3, 1);
        WithVar("TransformCount", 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        var count = DynamicVars["TransformCount"].IntValue;
        var eligibleCount = Owner.GetHand().Count(c => c != this && c.IsTransformable);
        if (eligibleCount == 0)
            return;

        var targetCount = Math.Min(count, eligibleCount);
        var chosen = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.IsTransformable,
            targetCount,
            targetCount);

        foreach (var card in chosen.ToList())
            await ShoreMagicianCmd.TransformCard(choiceContext, Owner, card, this);
    }
}
