using BaseLib.Abstracts;
using BaseLib.Extensions;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Abstract;

public abstract class ShoreMagicianPotionModel : CustomPotionModel
{
    public override string CustomPackedImagePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    public override string CustomPackedOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
}
