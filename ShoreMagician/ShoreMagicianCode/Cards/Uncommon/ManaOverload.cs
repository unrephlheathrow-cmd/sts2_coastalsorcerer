using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Powers;
using VoidCard = MegaCrit.Sts2.Core.Models.Cards.Void;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ManaOverload : ShoreMagicianCardModel
{
    public ManaOverload() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(12, 3);
        WithPower<MagicPower>(1, 0, true);
        WithTips(_ => [HoverTipFactory.FromCard<VoidCard>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ShoreMagicianCmd.AddGeneratedCardsToCombat<VoidCard>(
            Owner,
            1,
            PileType.Draw,
            CardPilePosition.Random);

        await CommonActions.ApplySelf<MagicPower>(choiceContext, this);
        await CommonActions.CardBlock(this, cardPlay);
    }
}
