using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class VortexForm : ShoreMagicianCardModel
{
    public VortexForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithVar("ExhaustCount", 3);
        WithPower<VortexFormPower>(1, 0, false);
        WithTips(_ => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var discardCount = Owner.GetDiscardPile().Count;
        if (discardCount > 0)
        {
            var toHand = await CommonActions.SelectCards(
                this,
                SelectionScreenPrompt,
                choiceContext,
                PileType.Discard,
                _ => true,
                0,
                discardCount);

            foreach (var card in toHand.ToList())
                await CardPileCmd.Add(card, PileType.Hand, CardPilePosition.Bottom);
        }

        var drawCount = Owner.GetDrawPile().Count;
        if (drawCount > 0)
        {
            var exhaustPrompt = new LocString("cards", $"{Id.Entry}.exhaustPrompt");
            var maxExhaust = Math.Min(DynamicVars["ExhaustCount"].IntValue, drawCount);
            var toExhaust = await CommonActions.SelectCards(
                this,
                exhaustPrompt,
                choiceContext,
                PileType.Draw,
                _ => true,
                0,
                maxExhaust);

            foreach (var card in toExhaust.ToList())
                await CardCmd.Exhaust(choiceContext, card);
        }

        await CommonActions.ApplySelf<VortexFormPower>(choiceContext, this);
    }
}
