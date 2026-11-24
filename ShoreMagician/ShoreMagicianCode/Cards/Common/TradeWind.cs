using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class TradeWind : ShoreMagicianCardModel
{
    public TradeWind() : base(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
    {
        WithCostUpgradeBy(-1);
        WithVar("StrengthLoss", 4);
        WithTips(_ => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromCard<SeaBreeze>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var opponents = Owner.GetAllOpponents();
        var strengthLoss = DynamicVars["StrengthLoss"].IntValue;
        if (opponents.Count > 0 && strengthLoss > 0)
            await PowerCmd.Apply<PiercingWailPower>(choiceContext, opponents, strengthLoss, Owner.Creature, this);

        await ShoreMagicianCmd.AddGeneratedCardsToCombat<SeaBreeze>(
            Owner,
            1,
            PileType.Hand,
            CardPilePosition.Bottom);
    }
}
