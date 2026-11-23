using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Core;

namespace ShoreMagician.Code.Relics;

[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class EnchantedBuckler : ShoreMagicianRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override Task BeforeCombatStart()
    {
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
            Status = RelicStatus.Active;

        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner || !ShoreMagicianModel.WasEnchantedAtPlayStart(cardPlay.Card))
            return;

        if (!ShoreMagicianModel.TryConsumeEnchantedBucklerThisTurn(Owner))
            return;

        Status = RelicStatus.Normal;
        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, 4m, ValueProp.Unpowered, null);
    }
}
