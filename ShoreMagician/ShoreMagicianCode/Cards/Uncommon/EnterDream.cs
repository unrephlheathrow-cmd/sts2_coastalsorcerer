using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class EnterDream : ShoreMagicianCardModel
{
    public EnterDream() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("SoulCount", 3);
        WithTips(card => [HoverTipFactory.FromCard<PoorSleep>(), HoverTipFactory.FromCard<Soul>(card.IsUpgraded)]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ShoreMagicianCmd.AddGeneratedCardsToCombat<PoorSleep>(
            Owner,
            1,
            PileType.Discard,
            CardPilePosition.Bottom);

        await ShoreMagicianCmd.AddGeneratedCardsToCombat<Soul>(
            Owner,
            DynamicVars["SoulCount"].IntValue,
            PileType.Draw,
            CardPilePosition.Random,
            upgrade: IsUpgraded);
    }
}
