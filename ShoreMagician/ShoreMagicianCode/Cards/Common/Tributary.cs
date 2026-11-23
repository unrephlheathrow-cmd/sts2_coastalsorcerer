using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Tributary : ShoreMagicianCardModel
{
    public Tributary() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(6, 0);
        WithCards(1, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var count = DynamicVars.Cards.IntValue;
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
