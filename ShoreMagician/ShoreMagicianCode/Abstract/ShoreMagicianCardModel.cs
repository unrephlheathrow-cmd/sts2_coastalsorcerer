using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Abstract;

public abstract class ShoreMagicianCardModel(
    int canonicalEnergyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    bool shouldShowInCardLibrary = true)
    : ConstructedCardModel(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    public sealed override string CustomPortraitPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected ConstructedCardModel WithPower<T>(int baseVal, int upgrade = 0, bool showTooltip = true)
        where T : PowerModel
    {
        WithVar(new DynamicVar(typeof(T).Name, baseVal).WithUpgrade(upgrade));
        if (showTooltip)
            WithTips(e => [HoverTipFactory.FromPower<T>(e.DynamicVars.Power<T>().IntValue)]);
        return this;
    }

    protected ConstructedCardModel WithEnchantmentTip<T>(int amount = 1)
        where T : EnchantmentModel
    {
        WithTips(_ => HoverTipFactory.FromEnchantment<T>(amount));
        return this;
    }
}
