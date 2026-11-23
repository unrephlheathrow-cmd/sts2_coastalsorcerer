using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Events;

namespace ShoreMagician.Code.Powers;

public sealed class WesterlyCurrentPower : ShoreMagicianPowerModel, IOnCardTransformed, IOnCardEnchanted, IOnEnchantmentCleared
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal) =>
        card.Owner?.Creature == Owner ? TriggerAsync(choiceContext) : Task.CompletedTask;

    public Task OnCardTransformed(PlayerChoiceContext ctx, Player player, CardModel originalCard, CardModel? newCard, CardModel? source) =>
        player.Creature == Owner ? TriggerAsync(ctx) : Task.CompletedTask;

    public Task OnCardEnchanted(PlayerChoiceContext ctx, Player player, CardModel targetCard, EnchantmentModel enchantment, int amount, CardModel? source) =>
        player.Creature == Owner ? TriggerAsync(ctx) : Task.CompletedTask;

    public Task OnEnchantmentCleared(PlayerChoiceContext ctx, Player player, CardModel targetCard, CardModel? source) =>
        player.Creature == Owner ? TriggerAsync(ctx) : Task.CompletedTask;

    private async Task TriggerAsync(PlayerChoiceContext ctx)
    {
        if (CombatState == null || CombatManager.Instance.IsEnding || Amount <= 0)
            return;

        var enemies = CombatState.HittableEnemies.Where(e => e.IsAlive).ToList();
        if (enemies.Count == 0)
            return;

        Flash();
        await CreatureCmd.Damage(ctx, enemies, Amount, ValueProp.Unpowered, Owner);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
