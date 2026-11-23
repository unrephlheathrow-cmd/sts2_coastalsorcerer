using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Relics;

namespace ShoreMagician.Code.Patches;

[HarmonyPatch]
public static class RelicMechanicsPatch
{
    private const string SlowAmountKey = "SlowAmount";
    private const string DisplayAmountKey = "DisplayAmount";

    private static bool HasActiveStarfish(SlowPower slowPower) =>
        slowPower.Owner.IsEnemy
        && (slowPower.Owner.CombatState?.Players.Any(p => p.GetRelic<Starfish>() != null) ?? false);

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
        {
            __result = filtered;
            talisman.Flash();
        }
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

    // 4. Starfish doubles the damage amplification of SlowPower (+10% -> +20% per card played)
    // and keeps SlowPower's badge counter, tooltip DynamicVar, and relic flash synchronized.
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
            || HasActiveStarfish(__instance);

        if (!hasStarfish)
            return;

        __result = 1m + (__result - 1m) * 2m;
    }

    [HarmonyPatch(typeof(SlowPower), nameof(SlowPower.DisplayAmount), MethodType.Getter)]
    [HarmonyPostfix]
    public static void StarfishSlowDisplayAmountPostfix(SlowPower __instance, ref int __result)
    {
        if (HasActiveStarfish(__instance))
            __result *= 2;
    }

    [HarmonyPatch(typeof(SlowPower), nameof(SlowPower.AfterCardPlayed))]
    [HarmonyPostfix]
    public static void StarfishSlowTooltipSyncPostfix(SlowPower __instance)
    {
        if (!HasActiveStarfish(__instance))
            return;

        if (__instance.DynamicVars.TryGetValue(SlowAmountKey, out var slowVar)
            && __instance.DynamicVars.TryGetValue(DisplayAmountKey, out var displayVar))
        {
            displayVar.BaseValue = slowVar.BaseValue * 20m;
            __instance.InvokeDisplayAmountChanged();
        }
    }

    [HarmonyPatch(typeof(SlowPower), nameof(SlowPower.AfterModifyingDamageAmount))]
    [HarmonyPostfix]
    public static void StarfishSlowFlashPostfix(SlowPower __instance)
    {
        if (!HasActiveStarfish(__instance))
            return;

        if (!__instance.DynamicVars.TryGetValue(SlowAmountKey, out var slowVar) || slowVar.IntValue <= 0)
            return;

        if (__instance.Owner.CombatState is not { } cs)
            return;

        foreach (var player in cs.Players)
            player.GetRelic<Starfish>()?.Flash();
    }

    // 5. Sailcloth rest site option custom icon path (RestSiteOption.IconPath is non-virtual).
    [HarmonyPatch(typeof(RestSiteOption), nameof(RestSiteOption.IconPath), MethodType.Getter)]
    [HarmonyPostfix]
    public static void SailclothRestSiteIconPathPostfix(RestSiteOption __instance, ref string __result)
    {
        if (__instance is SailclothEnchantRestSiteOption)
            __result = SailclothEnchantRestSiteOption.CustomIconPath;
    }

    // 6. Fix vanilla CardSelectCmd.FromDeckForEnchantment skipping NDeckEnchantSelectScreen
    // when cards.Count == 1 even though prefs.Cancelable or prefs.RequireManualConfirmation is true.
    [HarmonyPatch(
        typeof(CardSelectCmd),
        nameof(CardSelectCmd.FromDeckForEnchantment),
        typeof(IReadOnlyList<CardModel>),
        typeof(EnchantmentModel),
        typeof(int),
        typeof(CardSelectorPrefs))]
    [HarmonyPrefix]
    public static void FromDeckForEnchantmentSingleCardConfirmPrefix(
        IReadOnlyList<CardModel> cards,
        ref CardSelectorPrefs prefs)
    {
        if (TestMode.IsOn)
            return;

        if (cards.Count > 0
            && cards.Count <= prefs.MinSelect
            && (prefs.RequireManualConfirmation || prefs.Cancelable))
        {
            prefs = new CardSelectorPrefs(prefs.Prompt, 0, prefs.MaxSelect)
            {
                Cancelable = prefs.Cancelable,
                RequireManualConfirmation = prefs.RequireManualConfirmation,
                UnpoweredPreviews = prefs.UnpoweredPreviews
            };
        }
    }

    [HarmonyPatch(typeof(NDeckEnchantSelectScreen), nameof(NDeckEnchantSelectScreen.ShowScreen))]
    [HarmonyPrefix]
    public static void NDeckEnchantSelectScreenRestoreMinSelectPrefix(ref CardSelectorPrefs prefs)
    {
        if (prefs.MinSelect == 0
            && prefs.MaxSelect >= 1
            && (prefs.RequireManualConfirmation || prefs.Cancelable))
        {
            prefs = new CardSelectorPrefs(prefs.Prompt, prefs.MaxSelect)
            {
                Cancelable = prefs.Cancelable,
                RequireManualConfirmation = prefs.RequireManualConfirmation,
                UnpoweredPreviews = prefs.UnpoweredPreviews
            };
        }
    }
}
