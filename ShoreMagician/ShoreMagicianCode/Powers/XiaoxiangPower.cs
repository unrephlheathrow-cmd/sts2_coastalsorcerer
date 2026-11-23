using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class XiaoxiangPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player || Amount <= 0)
            return;

        Flash();
        await PlayerCmd.GainEnergy(Amount, player);
    }
}
