using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Shop relic: At Rest Sites, you can choose to Enchant a card in your deck with Swift 2.
/// </summary>
[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class Sailcloth : ShoreMagicianRelicModel
{
    public const int SwiftAmount = 2;

    public override RelicRarity Rarity => RelicRarity.Shop;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Swift>(SwiftAmount);

    public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
    {
        if (player != Owner)
            return false;

        options.Add(new SailclothEnchantRestSiteOption(player));
        return true;
    }
}
