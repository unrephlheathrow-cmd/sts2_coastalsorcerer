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
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class SeaBreezeCirculation : ShoreMagicianCardModel
{
    public SeaBreezeCirculation() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithPower<MagicPower>(1, 1, true);
        WithTips(_ => [HoverTipFactory.FromCard<SeaBreeze>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var handCount = Owner.GetHand().Count(c => c != this);
        if (handCount == 0)
            return;

        var chosen = (await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this,
            0,
            handCount)).ToList();

        if (chosen.Count == 0)
            return;

        var seaBreezeCount = chosen.Count(c => c is SeaBreeze);
        foreach (var card in chosen)
            await CardCmd.Exhaust(choiceContext, card);

        if (seaBreezeCount > 0)
        {
            var magicPerCurse = DynamicVars["MagicPower"].IntValue;
            await PowerCmd.Apply<MagicPower>(choiceContext, Owner.Creature, seaBreezeCount * magicPerCurse, Owner.Creature, this);
        }

        await ShoreMagicianCmd.AddGeneratedCardsToCombat<SeaBreeze>(
            Owner,
            chosen.Count,
            PileType.Discard,
            CardPilePosition.Bottom);
    }
}
