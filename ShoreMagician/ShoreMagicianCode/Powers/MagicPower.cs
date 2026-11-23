using MegaCrit.Sts2.Core.Entities.Powers;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class MagicPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
