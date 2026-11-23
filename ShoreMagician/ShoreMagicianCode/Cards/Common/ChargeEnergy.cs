using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ChargeEnergy : ShoreMagicianCardModel
{
    public ChargeEnergy() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithPower<TempMagicPower>(3, 0, true);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<TempMagicPower>(choiceContext, this);
    }
}
