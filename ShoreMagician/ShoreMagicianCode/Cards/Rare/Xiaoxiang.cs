using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Xiaoxiang : ShoreMagicianCardModel
{
    public Xiaoxiang() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithCostUpgradeBy(-1);
        WithPower<XiaoxiangPower>(1, 0, false);
        WithTips(_ => [HoverTipFactory.FromCard<Enthralled>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<XiaoxiangPower>(choiceContext, this);
        await ShoreMagicianCmd.AddGeneratedCardsToCombat<Enthralled>(
            Owner,
            1,
            PileType.Draw,
            CardPilePosition.Bottom);
    }
}
