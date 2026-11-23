using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class ConvergencePower : ShoreMagicianPowerModel
{
    private readonly List<ConvergenceEntry> _entries = [];
    private bool _isRemoving;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public void RegisterTarget(CardModel target, int energyGain, int drawCount)
    {
        _entries.Add(new ConvergenceEntry(target, energyGain, drawCount));
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner?.Creature != Owner || _isRemoving)
            return;

        var ctx = new BlockingPlayerChoiceContext();

        // If a tracked target entered Hand, trigger and consume it.
        if (oldPileType != PileType.Hand && card.Pile?.Type == PileType.Hand)
        {
            var matches = _entries.Where(e => ReferenceEquals(e.Target, card)).ToList();
            if (matches.Count > 0)
            {
                foreach (var entry in matches)
                    _entries.Remove(entry);

                foreach (var entry in matches)
                {
                    Flash();
                    if (entry.EnergyGain > 0)
                        await PlayerCmd.GainEnergy(entry.EnergyGain, card.Owner);
                    if (entry.DrawCount > 0)
                        await CardPileCmd.Draw(ctx, entry.DrawCount, card.Owner);
                }

                await SyncOrRemoveAsync(ctx);
                return;
            }
        }

        // Prune stale targets that left combat / were exhausted / were transformed away before entering Hand.
        var removedStale = PruneDeadTargets(card.Owner);
        if (removedStale > 0)
            await SyncOrRemoveAsync(ctx);
    }

    private int PruneDeadTargets(MegaCrit.Sts2.Core.Entities.Players.Player owner)
    {
        var allCards = owner.PlayerCombatState?.AllCards;
        return _entries.RemoveAll(e =>
            e.Target.Pile?.Type is PileType.Exhaust or PileType.None
            || (allCards != null && !allCards.Contains(e.Target)));
    }

    private async Task SyncOrRemoveAsync(PlayerChoiceContext ctx)
    {
        if (_isRemoving || !Owner.HasPower<ConvergencePower>())
            return;

        if (_entries.Count == 0)
        {
            _isRemoving = true;
            await PowerCmd.Remove(this);
            return;
        }

        var delta = _entries.Count - Amount;
        if (delta != 0)
            await PowerCmd.ModifyAmount(ctx, this, delta, Owner, null, true);
    }

    private sealed record ConvergenceEntry(CardModel Target, int EnergyGain, int DrawCount);
}
