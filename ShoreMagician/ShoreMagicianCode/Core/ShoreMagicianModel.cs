using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Events;

namespace ShoreMagician.Code.Core;

public sealed class ShoreMagicianModel() : CustomSingletonModel(HookType.Combat), IOnCardTransformed, IOnCardEnchanted
{
    public sealed record CardPlaySnapshot(
        CardModel Card,
        bool WasEnchantedAtStart,
        bool IsDoubled,
        bool ClaimedPreemptiveVoice,
        bool ClaimedDoubleNext);

    public sealed class TurnTracker
    {
        public bool TransformedAttackThisTurn;
        public bool PlayedOverflowThisCombat;
        public bool EnchantedBucklerTriggeredThisTurn;
        public int EnchantedCardsPlayedThisTurn;
        public bool CurseDrawnThisTurn;
        public int RedTideTriggersThisTurn;
        public int ClearInjusticeMagicGainedThisTurn;
        public bool PreemptiveClaimedInFlight;
        public int DoubleNextReservedCount;
        public List<CardPlaySnapshot> ActivePlays { get; } = [];
    }

    private static readonly SpireField<PlayerCombatState, TurnTracker> Trackers = new(() => new TurnTracker());

    public override bool ShouldReceiveCombatHooks => true;

    public static TurnTracker GetTracker(Player player) =>
        player.PlayerCombatState is { } state ? (Trackers[state] ??= new TurnTracker()) : new TurnTracker();

    public static int GetEnchantedCardsPlayedThisTurn(Player player) =>
        GetTracker(player).EnchantedCardsPlayedThisTurn;

    public static bool HasTransformedAttackThisTurn(Player player) =>
        GetTracker(player).TransformedAttackThisTurn;

    public static void MarkOverflowPlayed(Player player) =>
        GetTracker(player).PlayedOverflowThisCombat = true;

    public static bool HasPlayedOverflow(Player player) =>
        GetTracker(player).PlayedOverflowThisCombat;

    public static bool TryConsumeEnchantedBucklerThisTurn(Player player)
    {
        var tracker = GetTracker(player);
        if (tracker.EnchantedBucklerTriggeredThisTurn) return false;
        tracker.EnchantedBucklerTriggeredThisTurn = true;
        return true;
    }

    public static CardPlaySnapshot? FindActivePlay(CardModel card)
    {
        if (card.Owner == null) return null;
        var plays = GetTracker(card.Owner).ActivePlays;
        for (var i = plays.Count - 1; i >= 0; i--)
        {
            if (ReferenceEquals(plays[i].Card, card))
                return plays[i];
        }

        return null;
    }

    public static bool WasEnchantedAtPlayStart(CardModel card) =>
        FindActivePlay(card) is { } snap ? snap.WasEnchantedAtStart : card.Enchantment != null;

    public static bool TryConsumeFirstCurseDrawnThisTurn(Player player)
    {
        var tracker = GetTracker(player);
        if (tracker.CurseDrawnThisTurn) return false;
        tracker.CurseDrawnThisTurn = true;
        return true;
    }

    public static bool TryConsumeRedTideTrigger(Player player, int maxPerTurn = 4)
    {
        var tracker = GetTracker(player);
        if (tracker.RedTideTriggersThisTurn >= maxPerTurn) return false;
        tracker.RedTideTriggersThisTurn++;
        return true;
    }

    public static int ConsumeClearInjusticeMagicQuota(Player player, int requested, int maxPerTurn = 3)
    {
        if (requested <= 0) return 0;
        var tracker = GetTracker(player);
        var remaining = maxPerTurn - tracker.ClearInjusticeMagicGainedThisTurn;
        if (remaining <= 0) return 0;
        var granted = Math.Min(requested, remaining);
        tracker.ClearInjusticeMagicGainedThisTurn += granted;
        return granted;
    }

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != CombatSide.Player) return Task.CompletedTask;
        foreach (var player in combatState.Players)
        {
            if (!participants.Contains(player.Creature))
                continue;

            var t = GetTracker(player);
            t.TransformedAttackThisTurn = false;
            t.EnchantedBucklerTriggeredThisTurn = false;
            t.EnchantedCardsPlayedThisTurn = 0;
            t.CurseDrawnThisTurn = false;
            t.RedTideTriggersThisTurn = 0;
            t.ClearInjusticeMagicGainedThisTurn = 0;
            t.PreemptiveClaimedInFlight = false;
            t.DoubleNextReservedCount = 0;
            t.ActivePlays.Clear();
        }

        return Task.CompletedTask;
    }

    public Task OnCardTransformed(PlayerChoiceContext ctx, Player player, CardModel originalCard, CardModel? newCard, CardModel? source)
    {
        if (newCard?.Type == CardType.Attack)
            GetTracker(player).TransformedAttackThisTurn = true;
        return Task.CompletedTask;
    }

    public Task OnCardEnchanted(PlayerChoiceContext ctx, Player player, CardModel targetCard, EnchantmentModel enchantment, int amount, CardModel? source)
    {
        if (targetCard.Type == CardType.Attack)
            GetTracker(player).TransformedAttackThisTurn = true;
        return Task.CompletedTask;
    }
}
