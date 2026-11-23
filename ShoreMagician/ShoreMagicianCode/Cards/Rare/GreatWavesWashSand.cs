using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class GreatWavesWashSand : ShoreMagicianCardModel
{
    public GreatWavesWashSand() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<GreatWavesWashSandPower>(1, 0, false);
        WithTips(e => e.IsUpgraded
            ? [HoverTipFactory.FromCard<SeaBreeze>(), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)]
            : [HoverTipFactory.FromCard<SeaBreeze>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<GreatWavesWashSandPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        if (IsUpgraded)
            await PowerCmd.Apply<GreatWavesWashSandUpgradedPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }
}
