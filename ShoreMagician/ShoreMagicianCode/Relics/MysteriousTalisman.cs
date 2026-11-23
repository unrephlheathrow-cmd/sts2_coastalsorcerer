using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Common relic: Curses cannot be transformed into Enthralled, Normality, Bad Luck, or Debt.
/// Filtering is enforced by RelicMechanicsPatch on CardFactory.GetDefaultTransformationOptions.
/// </summary>
[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class MysteriousTalisman : ShoreMagicianRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
}
