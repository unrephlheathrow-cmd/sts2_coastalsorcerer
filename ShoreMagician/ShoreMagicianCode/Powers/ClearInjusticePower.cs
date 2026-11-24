using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Core;
using ShoreMagician.Code.Events;

namespace ShoreMagician.Code.Powers;

public sealed class ClearInjusticePower : ShoreMagicianPowerModel, IOnCardTransformed
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<MagicPower>()];

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner?.Creature != Owner || card.Type != CardType.Curse)
            return;

        await TriggerAsync(choiceContext);
    }

    public async Task OnCardTransformed(PlayerChoiceContext ctx, Player player, CardModel originalCard, CardModel? newCard, CardModel? source)
    {
        if (player.Creature != Owner || originalCard.Type != CardType.Curse)
            return;

        await TriggerAsync(ctx);
    }

    private async Task TriggerAsync(PlayerChoiceContext ctx)
    {
        if (CombatManager.Instance.IsEnding)
            return;

        Flash();
        if (Amount > 0 && Owner.Player != null)
        {
            await CardPileCmd.Draw(ctx, Amount, Owner.Player);
            var magicToGrant = ShoreMagicianModel.ConsumeClearInjusticeMagicQuota(Owner.Player, Amount, 3);
            if (magicToGrant > 0)
                await PowerCmd.Apply<MagicPower>(ctx, Owner, magicToGrant, Owner, null);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
