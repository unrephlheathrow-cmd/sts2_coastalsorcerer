using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class MoonOfLighthouse : ShoreMagicianCardModel
{
    public MoonOfLighthouse() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        WithCostUpgradeBy(-1);
        WithPower<MoonOfLighthousePower>(1, 0, false);
        WithTips(_ =>
        [
            HoverTipFactory.FromCard<Fuel>(),
            HoverTipFactory.FromCard<MinionSacrifice>(),
            HoverTipFactory.FromCard<Joy>(),
            HoverTipFactory.FromCard<Soul>(),
            HoverTipFactory.FromCard<Dazed>(),
        ]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<MoonOfLighthousePower>(choiceContext, this);
    }
}
