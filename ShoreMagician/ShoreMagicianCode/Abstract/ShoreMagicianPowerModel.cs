using BaseLib.Abstracts;
using BaseLib.Extensions;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Abstract;

public abstract class ShoreMagicianPowerModel : CustomPowerModel
{
    public sealed override string CustomPackedIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();

    public sealed override string CustomBigIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_big.png".BigPowerImagePath();
}
