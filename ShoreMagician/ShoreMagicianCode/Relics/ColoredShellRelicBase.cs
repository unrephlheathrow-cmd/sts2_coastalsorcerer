using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Base class for RedShell, OrangeShell, and WhiteShell.
/// Modeled directly on vanilla LastingCandy: every 3 combat rewards, appends 1 extra Power card
/// from TargetCardPool to the combat card reward choices.
/// </summary>
public abstract class ColoredShellRelicBase : ShoreMagicianRelicModel
{
    protected const int TriggerInterval = 3;

    private bool _isActivating;
    private int _combatRewardsSeen;

    protected abstract CardPoolModel TargetCardPool { get; }

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override bool ShowCounter => true;

    public override int DisplayAmount => _isActivating ? TriggerInterval : CombatRewardsSeen % TriggerInterval;

    private bool IsActivating
    {
        get => _isActivating;
        set
        {
            AssertMutable();
            _isActivating = value;
            UpdateDisplay();
        }
    }

    [SavedProperty]
    public int CombatRewardsSeen
    {
        get => _combatRewardsSeen;
        set
        {
            AssertMutable();
            _combatRewardsSeen = value;
            UpdateDisplay();
        }
    }

    private bool IsInTriggeringCombat =>
        CombatRewardsSeen > 0 && CombatRewardsSeen % TriggerInterval == TriggerInterval - 1;

    private void UpdateDisplay()
    {
        Status = IsInTriggeringCombat || _isActivating ? RelicStatus.Active : RelicStatus.Normal;
        InvokeDisplayAmountChanged();
    }

    public override bool TryModifyCardRewardOptions(
        Player player,
        List<CardCreationResult> cardRewardOptions,
        CardCreationOptions creationOptions)
    {
        if (player != Owner)
            return false;

        if (creationOptions.Source != CardCreationSource.Encounter)
            return false;

        if (!IsInTriggeringCombat)
            return false;

        if (!creationOptions.Flags.HasFlag(CardCreationFlags.IsCardReward)
            || !creationOptions.Flags.HasFlag(CardCreationFlags.IsFromCombat))
            return false;

        var targetPool = TargetCardPool;
        var unlockedPowers = targetPool
            .GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
            .Where(c => c.Type == CardType.Power)
            .ToList();

        if (unlockedPowers.Count == 0)
            return false;

        var allowDupes = !unlockedPowers.Any(c => cardRewardOptions.TrueForAll(o => o.Card.Id != c.Id));

        var extraOptions = new CardCreationOptions(
            [targetPool],
            CardCreationSource.Other,
            creationOptions.RarityOdds,
            c => c.Type == CardType.Power && (allowDupes || cardRewardOptions.TrueForAll(o => o.Card.Id != c.Id)))
            .WithFlags(CardCreationFlags.NoModifyHooks | CardCreationFlags.NoCardPoolModifications);

        var card = CardFactory.CreateForReward(Owner, 1, extraOptions).FirstOrDefault()?.Card;
        if (card == null)
            return false;

        var result = new CardCreationResult(card);
        result.ModifyCard(card, this);
        cardRewardOptions.Add(result);
        return true;
    }

    public override Task BeforeCombatRewardOffered(RewardsSet rewards, CombatRoom room)
    {
        if (rewards.Player != Owner)
            return Task.CompletedTask;

        if (rewards.Rewards.All(r => r is not CardReward))
            return Task.CompletedTask;

        if (IsInTriggeringCombat)
            TaskHelper.RunSafely(DoActivateVisuals());

        CombatRewardsSeen++;
        return Task.CompletedTask;
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Task.Delay(1000);
        IsActivating = false;
    }
}
