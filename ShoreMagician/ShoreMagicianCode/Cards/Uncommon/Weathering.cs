using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Weathering : ShoreMagicianCardModel
{
    public Weathering() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithVar("SeaBreezeCount", 4, -1);
        WithTips(_ => [HoverTipFactory.FromCard<SeaBreeze>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cursesToExhaust = Owner.GetDrawPile()
            .Concat(Owner.GetHand())
            .Where(c => c != this && c.Type == CardType.Curse)
            .ToList();

        foreach (var curse in cursesToExhaust)
            await CardCmd.Exhaust(choiceContext, curse);

        await ShoreMagicianCmd.AddGeneratedCardsToCombat<SeaBreeze>(
            Owner,
            DynamicVars["SeaBreezeCount"].IntValue,
            PileType.Discard,
            CardPilePosition.Bottom);
    }
}
