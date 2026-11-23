using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class WesterlyCurrent : ShoreMagicianCardModel
{
    public WesterlyCurrent() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithPower<WesterlyCurrentPower>(5, 2, false);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<WesterlyCurrentPower>(choiceContext, this);
    }
}
