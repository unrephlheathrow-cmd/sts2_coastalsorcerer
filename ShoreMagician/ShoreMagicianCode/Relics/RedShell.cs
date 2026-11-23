using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Uncommon relic: Every 3 combats, add 1 Ironclad Power card to card rewards.
/// </summary>
[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class RedShell : ColoredShellRelicBase
{
    protected override CardPoolModel TargetCardPool => ModelDb.CardPool<IroncladCardPool>();
}
