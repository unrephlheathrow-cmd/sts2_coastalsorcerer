using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Rooms;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Rewards;

namespace ShoreMagician.Code.Powers;

public sealed class FountainRepayPower : ShoreMagicianPowerModel
{
    private sealed class CastCountdown(int remainingTurns)
    {
        public int RemainingTurns = remainingTurns;
        public bool ActiveThisTurn;
    }

    private readonly List<CastCountdown> _casts = [];
    private int _energySpentThisTurn;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipFactory.FromEnchantment<PerfectFit>(1)
            .Append(HoverTipFactory.FromPower<FountainRepayRewardPower>());

    public void RegisterCast()
    {
        // Starts monitoring on the next two player turns without disrupting casts already monitoring this turn.
        _casts.Add(new CastCountdown(2));
    }

    public int DesiredDisplayAmount => _casts.Count > 0 ? _casts.Max(c => c.RemainingTurns) : 0;

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != Owner.Side || !participants.Contains(Owner))
            return Task.CompletedTask;

        _energySpentThisTurn = 0;
        foreach (var cast in _casts)
            cast.ActiveThisTurn = true;

        return Task.CompletedTask;
    }

    public override async Task AfterEnergySpent(CardModel card, int amount)
    {
        if (card.Owner?.Creature != Owner || amount <= 0 || !_casts.Any(c => c.ActiveThisTurn))
            return;

        _energySpentThisTurn += amount;
        if (_energySpentThisTurn <= 3)
            return;

        _casts.RemoveAll(c => c.ActiveThisTurn);
        Flash();

        if (_casts.Count == 0)
        {
            await PowerCmd.Remove(this);
            return;
        }

        var delta = DesiredDisplayAmount - Amount;
        if (delta != 0)
            await PowerCmd.ModifyAmount(new BlockingPlayerChoiceContext(), this, delta, Owner, null, true);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side || !participants.Contains(Owner))
            return;

        var completed = 0;
        for (var i = _casts.Count - 1; i >= 0; i--)
        {
            var cast = _casts[i];
            if (!cast.ActiveThisTurn)
                continue;

            cast.ActiveThisTurn = false;
            cast.RemainingTurns--;
            if (cast.RemainingTurns <= 0)
            {
                _casts.RemoveAt(i);
                completed++;
            }
        }

        if (completed > 0)
        {
            Flash();
            await PowerCmd.Apply<FountainRepayRewardPower>(choiceContext, Owner, completed, Owner, null);
        }

        if (_casts.Count == 0)
        {
            await PowerCmd.Remove(this);
            return;
        }

        var delta = DesiredDisplayAmount - Amount;
        if (delta != 0)
            await PowerCmd.ModifyAmount(choiceContext, this, delta, Owner, null, true);
    }

    // Runs BEFORE Player.AfterCombatEnd strips all powers; guards against fleeing/defeat.
    public override Task AfterCombatEnd(CombatRoom room)
    {
        var player = Owner.Player;
        if (player == null || Owner.IsDead || !IsCombatVictory(room))
            return Task.CompletedTask;

        var finishingCount = _casts.Count(c => c.ActiveThisTurn && c.RemainingTurns == 1 && _energySpentThisTurn <= 3);
        if (finishingCount > 0)
        {
            Flash();
            for (var i = 0; i < finishingCount; i++)
                room.AddExtraReward(player, new EnchantPerfectFitReward(player));
            _casts.RemoveAll(c => c.ActiveThisTurn && c.RemainingTurns == 1);
        }

        return Task.CompletedTask;
    }

    internal static bool IsCombatVictory(CombatRoom room) =>
        !CombatManager.Instance.IsAboutToLose
        && room.CombatState.Enemies.All(e => e.IsDead);
}

public sealed class FountainRepayRewardPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<PerfectFit>(1);

    public override Task AfterCombatEnd(CombatRoom room)
    {
        var player = Owner.Player;
        if (player == null || Owner.IsDead || !FountainRepayPower.IsCombatVictory(room))
            return Task.CompletedTask;

        Flash();
        for (var i = 0; i < Amount; i++)
            room.AddExtraReward(player, new EnchantPerfectFitReward(player));

        return Task.CompletedTask;
    }
}
