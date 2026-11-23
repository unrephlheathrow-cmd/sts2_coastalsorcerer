using BaseLib.Abstracts;
using BaseLib.Extensions;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Abstract;

public abstract class ShoreMagicianRelicModel : CustomRelicModel
{
    protected override string BigIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_big.png".BigRelicImagePath();

    public override string PackedIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
}
