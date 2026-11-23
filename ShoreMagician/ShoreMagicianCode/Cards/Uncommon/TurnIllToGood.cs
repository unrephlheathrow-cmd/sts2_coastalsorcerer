using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class TurnIllToGood : ShoreMagicianCardModel
{
    public TurnIllToGood() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithPower<TurnIllToGoodPower>(2, 0, false);
        WithTip(typeof(MagicPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<TurnIllToGoodPower>(choiceContext, this);
    }
}
