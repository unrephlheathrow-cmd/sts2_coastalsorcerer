using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Dredge : ShoreMagicianCardModel
{
    public Dredge() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCostUpgradeBy(-1);
        WithKeywords(CardKeyword.Exhaust);
        WithVar("TransformCount", 3);
        WithCards(2, 0);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var transformCount = DynamicVars["TransformCount"].IntValue;
        var chosen = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.IsTransformable,
            0,
            transformCount);

        foreach (var card in chosen.ToList())
            await ShoreMagicianCmd.TransformCard(choiceContext, Owner, card, this);

        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }
}
