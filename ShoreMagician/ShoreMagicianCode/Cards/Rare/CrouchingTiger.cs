using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class CrouchingTiger : ShoreMagicianCardModel
{
    public CrouchingTiger() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithPower<CrouchingTigerPower>(1, 0, false);
        WithTips(_ =>
        [
            HoverTipFactory.FromPower<SlowPower>(),
            HoverTipFactory.FromPower<VulnerablePower>(),
            HoverTipFactory.FromPower<WeakPower>()
        ]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<CrouchingTigerPower>(choiceContext, this);
    }
}
