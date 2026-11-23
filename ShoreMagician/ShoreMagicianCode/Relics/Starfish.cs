using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Rare relic: Doubles the damage amplification granted by Slow (from +10% per card to +20% per card).
/// Amplification is applied by RelicMechanicsPatch on SlowPower.ModifyDamageMultiplicative.
/// </summary>
[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class Starfish : ShoreMagicianRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SlowPower>()];
}
