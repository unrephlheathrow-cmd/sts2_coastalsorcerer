using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class OceanCurrent : ShoreMagicianCardModel
{
    public OceanCurrent() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        WithCostUpgradeBy(-1);
        WithPower<OceanCurrentPower>(1, 0, false);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<OceanCurrentPower>(choiceContext, this);
    }
}
