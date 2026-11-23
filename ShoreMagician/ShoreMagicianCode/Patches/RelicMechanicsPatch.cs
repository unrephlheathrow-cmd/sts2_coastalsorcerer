using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Relics;

namespace ShoreMagician.Code.Patches;

[HarmonyPatch]
public static class RelicMechanicsPatch
{
    // 1. Register Spell Booklet (EnchantedBuckler) -> Arcane Lexicon (ArcaneLexicon) in Touch of Orobas.
    [HarmonyPatch(typeof(TouchOfOrobas), nameof(TouchOfOrobas.RefinementUpgrades), MethodType.Getter)]
    [HarmonyPostfix]
    public static void TouchOfOrobasRefinementUpgradesPostfix(Dictionary<ModelId, RelicModel> __result)
    {
        __result[ModelDb.Relic<EnchantedBuckler>().Id] = ModelDb.Relic<ArcaneLexicon>();
    }

    // 2. Mysterious Talisman prevents Curses from transforming into Enthralled, Normality, Bad Luck, or Debt.
    [HarmonyPatch(typeof(CardFactory), nameof(CardFactory.GetDefaultTransformationOptions))]
    [HarmonyPostfix]
    public static void MysteriousTalismanCurseOptionsPostfix(CardModel original, ref IEnumerable<CardModel> __result)
    {
        if (original.Type != CardType.Curse)
            return;

        var talisman = original.Owner?.GetRelic<MysteriousTalisman>();
        if (talisman == null)
            return;

        var originalList = __result.ToList();
        var filtered = originalList
            .Where(c => c is not (Enthralled or Normality or BadLuck or Debt))
            .ToList();

        if (filtered.Count > 0)
            __result = filtered;
    }

    // 3. Moon Sand makes Sea Breeze cost 0 Energy in combat (bypassing CardEnergyCost's _base < 0 early return).
    [HarmonyPatch(typeof(CardEnergyCost), nameof(CardEnergyCost.GetWithModifiers))]
    [HarmonyPostfix]
    public static void MoonSandSeaBreezeCostPostfix(CardEnergyCost __instance, ref int __result)
    {
        if (__instance._card is SeaBreeze sb
            && sb.IsInCombat
            && sb.Owner?.GetRelic<MoonSand>() != null)
        {
            __result = 0;
        }
    }

    // 4. Starfish doubles the damage amplification of SlowPower (+10% -> +20% per card played).
    [HarmonyPatch(typeof(SlowPower), nameof(SlowPower.ModifyDamageMultiplicative))]
    [HarmonyPostfix]
    public static void StarfishSlowDamagePostfix(
        SlowPower __instance,
        Creature? target,
        ValueProp props,
        Creature? dealer,
        ref decimal __result)
    {
        if (__result <= 1m || target != __instance.Owner || !props.IsPoweredAttack())
            return;

        if (!__instance.Owner.IsEnemy)
            return;

        var hasStarfish = dealer?.Player?.GetRelic<Starfish>() != null
            || dealer?.PetOwner?.GetRelic<Starfish>() != null
            || (__instance.Owner.CombatState?.Players.Any(p => p.GetRelic<Starfish>() != null) ?? false);

        if (!hasStarfish)
            return;

        __result = 1m + (__result - 1m) * 2m;
    }

    // 5. Sailcloth rest site option custom icon path (RestSiteOption.IconPath is non-virtual).
    [HarmonyPatch(typeof(RestSiteOption), nameof(RestSiteOption.IconPath), MethodType.Getter)]
    [HarmonyPostfix]
    public static void SailclothRestSiteIconPathPostfix(RestSiteOption __instance, ref string __result)
    {
        if (__instance is SailclothEnchantRestSiteOption)
            __result = SailclothEnchantRestSiteOption.CustomIconPath;
    }
}
