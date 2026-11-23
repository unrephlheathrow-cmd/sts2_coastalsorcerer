using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class ExhaustingStrikeCostPower : ShoreMagicianPowerModel
{
    private readonly Dictionary<CardModel, int> _excludedCounts = new(ReferenceEqualityComparer.Instance);

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public void RegisterExcluded(CardModel card)
    {
        _excludedCounts[card] = _excludedCounts.TryGetValue(card, out var count) ? count + 1 : 1;
    }

    // Mirrors BorrowedTimePower.TryModifyEnergyCostInCombat, excluding only the stacks applied by each card itself.
    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner?.Creature != Owner)
        {
            modifiedCost = originalCost;
            return false;
        }

        var excluded = _excludedCounts.TryGetValue(card, out var count) ? count : 0;
        var effectiveIncrease = Amount - excluded;
        if (effectiveIncrease <= 0)
        {
            modifiedCost = originalCost;
            return false;
        }

        modifiedCost = originalCost + effectiveIncrease;
        return true;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == Owner.Side && participants.Contains(Owner))
            await PowerCmd.Remove(this);
    }
}
