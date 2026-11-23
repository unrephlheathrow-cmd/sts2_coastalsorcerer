using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Rooms;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Rewards;

namespace ShoreMagician.Code.Powers;

public sealed class ExperienceSummaryPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Adroit>(2);

    public override Task AfterCombatEnd(CombatRoom room)
    {
        var player = Owner.Player;
        if (player == null || Owner.IsDead || !FountainRepayPower.IsCombatVictory(room))
            return Task.CompletedTask;

        Flash();
        for (var i = 0; i < Amount; i++)
            room.AddExtraReward(player, new EnchantAdroitReward(player, 2));

        return Task.CompletedTask;
    }
}

public sealed class ExperienceSummaryUpgradedPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Adroit>(3);

    public override Task AfterCombatEnd(CombatRoom room)
    {
        var player = Owner.Player;
        if (player == null || Owner.IsDead || !FountainRepayPower.IsCombatVictory(room))
            return Task.CompletedTask;

        Flash();
        for (var i = 0; i < Amount; i++)
            room.AddExtraReward(player, new EnchantAdroitReward(player, 3));

        return Task.CompletedTask;
    }
}
