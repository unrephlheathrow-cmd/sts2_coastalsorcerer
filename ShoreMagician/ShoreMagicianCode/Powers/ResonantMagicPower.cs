using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Core;

namespace ShoreMagician.Code.Powers;

public sealed class ResonantMagicPower : ShoreMagicianPowerModel
{
    public CardModel? IgnoreCurrentPlayCard { get; set; }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<TempMagicPower>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner)
            return;

        if (ReferenceEquals(cardPlay.Card, IgnoreCurrentPlayCard))
        {
            IgnoreCurrentPlayCard = null;
            return;
        }

        if (!ShoreMagicianModel.WasEnchantedAtPlayStart(cardPlay.Card))
            return;

        Flash();
        await PowerCmd.Apply<TempMagicPower>(choiceContext, Owner, Amount, Owner, null);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
