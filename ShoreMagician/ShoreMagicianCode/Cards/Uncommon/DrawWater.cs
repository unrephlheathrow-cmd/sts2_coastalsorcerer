using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class DrawWater : ShoreMagicianCardModel
{
    public DrawWater() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithEnergy(3);
        WithPower<WeakPower>(1, 0, true);
        WithPower<VulnerablePower>(1, 0, true);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        WithEnergyTip();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await CommonActions.ApplySelf<WeakPower>(choiceContext, this);
        await CommonActions.ApplySelf<VulnerablePower>(choiceContext, this);
    }
}
