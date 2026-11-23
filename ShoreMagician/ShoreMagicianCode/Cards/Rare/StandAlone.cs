using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class StandAlone : ShoreMagicianCardModel
{
    public StandAlone() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithBlock(12, 0);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Discard,
            _ => true);

        if (chosen == null)
            return;

        await CardPileCmd.Add(chosen, PileType.Draw, CardPilePosition.Bottom);
        chosen.EnergyCost.SetUntilPlayed(0, reduceOnly: true);
    }
}
