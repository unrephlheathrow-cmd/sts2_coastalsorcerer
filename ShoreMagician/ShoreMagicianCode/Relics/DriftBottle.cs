using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Rare relic: The first time you play a Power card each turn, play the bottom card of your draw pile.
/// </summary>
[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class DriftBottle : ShoreMagicianRelicModel
{
    private bool _triggeredThisTurn;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override Task BeforeCombatStart()
    {
        _triggeredThisTurn = false;
        Status = RelicStatus.Active;
        return Task.CompletedTask;
    }

    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side == CombatSide.Player && participants.Contains(Owner.Creature))
        {
            _triggeredThisTurn = false;
            Status = RelicStatus.Active;
        }

        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        _triggeredThisTurn = false;
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (_triggeredThisTurn || cardPlay.Card.Owner != Owner || cardPlay.Card.Type != CardType.Power)
            return;

        if (Owner.Creature.CombatState is not { } cs || CombatManager.Instance.IsOverOrEnding)
            return;

        _triggeredThisTurn = true;
        Status = RelicStatus.Normal;

        var drawCards = PileType.Draw.GetPile(Owner).Cards;
        if (drawCards.Count == 0)
            return;

        var bottomCard = drawCards[^1];
        Creature? target = null;
        if (bottomCard.TargetType == TargetType.AnyEnemy)
        {
            if (cs.HittableEnemies.Count == 0)
                return;

            target = Owner.RunState.Rng.CombatTargets.NextItem(cs.HittableEnemies);
        }

        Flash();
        await CardCmd.AutoPlay(choiceContext, bottomCard, target, AutoPlayType.Default);
    }
}
