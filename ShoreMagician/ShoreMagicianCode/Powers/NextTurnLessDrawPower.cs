using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class NextTurnLessDrawPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner.Player)
            return count;

        return count - Amount;
    }

    public override async Task AfterModifyingHandDraw()
    {
        await PowerCmd.Remove(this);
    }
}
