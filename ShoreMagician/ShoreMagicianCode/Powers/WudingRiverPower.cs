using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public class WudingRiverPower : ShoreMagicianPowerModel
{
    // Lazy factories avoid ModelDb static-initialization ordering hazards.
    // Excludes deprecated/test, harmful (Corrupted/Slither/TezcatarasEmber), and combat-start-only (Imbued).
    private static readonly Func<EnchantmentModel>[] SafeRandomEnchantmentFactories =
    [
        static () => ModelDb.Enchantment<Sharp>(),
        static () => ModelDb.Enchantment<Nimble>(),
        static () => ModelDb.Enchantment<Adroit>(),
        static () => ModelDb.Enchantment<Vigorous>(),
        static () => ModelDb.Enchantment<Momentum>(),
        static () => ModelDb.Enchantment<Swift>(),
        static () => ModelDb.Enchantment<Sown>(),
        static () => ModelDb.Enchantment<Goopy>(),
        static () => ModelDb.Enchantment<Inky>(),
        static () => ModelDb.Enchantment<Glam>(),
        static () => ModelDb.Enchantment<Instinct>(),
        static () => ModelDb.Enchantment<PerfectFit>(),
        static () => ModelDb.Enchantment<RoyallyApproved>(),
        static () => ModelDb.Enchantment<SoulsPower>(),
        static () => ModelDb.Enchantment<Spiral>(),
        static () => ModelDb.Enchantment<Steady>(),
        static () => ModelDb.Enchantment<SlumberingEssence>()
    ];

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public static bool IsActiveFor(Player? player) =>
        player?.Creature.HasPower<WudingRiverPower>() == true
        || player?.Creature.HasPower<WudingRiverUpgradedPower>() == true;

    public static bool IsUpgradedActiveFor(Player? player) =>
        player?.Creature.HasPower<WudingRiverUpgradedPower>() == true;

    public static bool IsNumericStackable(EnchantmentModel enchantment) =>
        enchantment.ShowAmount || enchantment is Sown or Goopy;

    public static void OnCardTransformedInCombat(CardModel replacement)
    {
        if (!replacement.IsInCombat || replacement.Owner is not { } owner || !IsActiveFor(owner))
            return;

        PowerModel? activePower = owner.Creature.GetPower<WudingRiverUpgradedPower>()
            ?? (PowerModel?)owner.Creature.GetPower<WudingRiverPower>();

        var didModify = false;
        if (IsUpgradedActiveFor(owner) && replacement.IsUpgradable)
        {
            CardCmd.Upgrade(replacement);
            didModify = true;
        }

        if (replacement.Enchantment == null)
        {
            var candidates = SafeRandomEnchantmentFactories
                .Select(f => f())
                .Where(e => e.CanEnchant(replacement))
                .ToList();

            var chosen = candidates.Count > 0
                ? owner.RunState.Rng.CombatCardGeneration.NextItem(candidates)
                : null;

            if (chosen != null)
            {
                var amount = chosen is Sharp or Nimble or Adroit or Vigorous or Momentum or Goopy ? 2m : 1m;
                var applied = CardCmd.Enchant(chosen.ToMutable(), replacement, amount);
                if (applied is Adroit adroit)
                    adroit.RecalculateValues();
                didModify = true;
            }
        }

        if (didModify)
            activePower?.Flash();
    }
}

public sealed class WudingRiverUpgradedPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WudingRiverPower>()];
}
