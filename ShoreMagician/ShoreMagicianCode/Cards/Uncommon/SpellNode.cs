using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class SpellNode : ShoreMagicianCardModel
{
    public SpellNode() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(3, 0);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var drawCopy = CreateClone();
        var discardCopy = CreateClone();
        var drawRes = await CardPileCmd.AddGeneratedCardToCombat(drawCopy, PileType.Draw, Owner, CardPilePosition.Random);
        var discardRes = await CardPileCmd.AddGeneratedCardToCombat(discardCopy, PileType.Discard, Owner, CardPilePosition.Bottom);
        CardCmd.PreviewCardPileAdd([drawRes, discardRes]);
    }
}
