using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Core;

namespace ShoreMagician.Code.Powers;

public sealed class TurnIllToGoodPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<MagicPower>()];

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner?.Creature != Owner || card.Type != CardType.Curse)
            return;

        if (!ShoreMagicianModel.TryConsumeFirstCurseDrawnThisTurn(card.Owner))
            return;

        Flash();
        await PowerCmd.Apply<MagicPower>(choiceContext, Owner, Amount, Owner, null);
    }
}
