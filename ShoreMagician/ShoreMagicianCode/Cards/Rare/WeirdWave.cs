using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class WeirdWave : ShoreMagicianCardModel
{
    public WeirdWave() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(2, 1);
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        await PowerCmd.Apply<WeirdWaveCostPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);

        var drawCopy = CreateClone();
        var discardCopy = CreateClone();
        var drawRes = await CardPileCmd.AddGeneratedCardToCombat(drawCopy, PileType.Draw, Owner, CardPilePosition.Random);
        var discardRes = await CardPileCmd.AddGeneratedCardToCombat(discardCopy, PileType.Discard, Owner, CardPilePosition.Bottom);
        CardCmd.PreviewCardPileAdd([drawRes, discardRes]);
    }
}
