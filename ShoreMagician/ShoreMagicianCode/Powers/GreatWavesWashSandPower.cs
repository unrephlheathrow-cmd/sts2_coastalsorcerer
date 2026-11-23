using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;

namespace ShoreMagician.Code.Powers;

public sealed class GreatWavesWashSandPower : ShoreMagicianPowerModel
{
    private bool _triggeredThisTurn;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<SeaBreeze>()];

    public override Task BeforeSideTurnStart(
        MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == Owner.Side && participants.Contains(Owner))
            _triggeredThisTurn = false;

        return Task.CompletedTask;
    }

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (_triggeredThisTurn || card is not SeaBreeze)
            return;

        if (card.Owner?.Creature != Owner)
            return;

        if (Owner.Player == null || Amount <= 0)
            return;

        _triggeredThisTurn = true;
        Flash();
        await PlayerCmd.GainEnergy(Amount, Owner.Player);
    }
}

public sealed class GreatWavesWashSandUpgradedPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<SeaBreeze>()];

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player?.PlayerCombatState != null)
        {
            foreach (var card in Owner.Player.PlayerCombatState.AllCards)
                ConfigureSeaBreezeCost(card);
        }

        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner?.Creature == Owner)
            ConfigureSeaBreezeCost(card);

        return Task.CompletedTask;
    }

    public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
    {
        if (card is not SeaBreeze || card.Owner?.Creature != Owner)
            return false;

        var changed = keywords.Remove(CardKeyword.Unplayable);
        changed |= keywords.Add(CardKeyword.Exhaust);
        return changed;
    }

    private static void ConfigureSeaBreezeCost(CardModel card)
    {
        if (card is SeaBreeze && card.EnergyCost.GetWithModifiers(CostModifiers.None) < 0)
            card.EnergyCost.SetCustomBaseCost(1);
    }
}
