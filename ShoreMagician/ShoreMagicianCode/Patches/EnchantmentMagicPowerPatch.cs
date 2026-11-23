using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Cards.Common;
using ShoreMagician.Code.Cards.Uncommon;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Core;
using ShoreMagician.Code.Events;
using ShoreMagician.Code.Extensions;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Patches;

[HarmonyPatch]
public static class EnchantmentMagicPowerPatch
{
    [ThreadStatic]
    private static bool _allowWudingStacking;

    [ThreadStatic]
    private static int _allowReplaceEnchantmentDepth;

    public static TResult RunAllowReplaceEnchantment<TResult>(Func<TResult> func)
    {
        _allowReplaceEnchantmentDepth++;
        try
        {
            return func();
        }
        finally
        {
            _allowReplaceEnchantmentDepth--;
        }
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.Tags), MethodType.Getter)]
    [HarmonyPostfix]
    public static void CardTagsPostfix(CardModel __instance, ref IEnumerable<CardTag> __result)
    {
        if (__instance is ManaStrike ms && ms.ShouldCountAsStrike && !__result.Contains(CardTag.Strike))
            __result = __result.Append(CardTag.Strike);
    }

    // 0a. Enchanted ShoreMagician cards reject being enchanted again until cleared,
    // unless replacing explicitly or WudingRiver is active (same-type numeric stacks; different-type overwrites).
    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.IsStackable), MethodType.Getter)]
    [HarmonyPostfix]
    public static void EnchantmentIsStackablePostfix(ref bool __result)
    {
        if (_allowWudingStacking)
            __result = true;
    }

    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.CanEnchant), [typeof(CardModel)])]
    [HarmonyPrefix]
    public static bool EnchantmentCanEnchantPrefix(EnchantmentModel __instance, CardModel card, ref bool __result, out bool __state)
    {
        __state = false;
        if (card.Enchantment == null)
            return true;

        var isSameType = card.Enchantment.GetType() == __instance.GetType();
        var wudingActive = WudingRiverPower.IsActiveFor(card.Owner);

        // Allow same-type numeric stacking under WudingRiver (vanilla CanEnchant passes via IsStackable = true).
        if (isSameType && wudingActive && WudingRiverPower.IsNumericStackable(__instance))
        {
            __state = true;
            _allowWudingStacking = true;
            return true;
        }

        // Allow explicit replacement (CanReplaceEnchantmentWith<T>) or WudingRiver different-type overwrite,
        // while preserving all non-enchantment base checks (and letting subclass overrides like Nimble still run).
        if (_allowReplaceEnchantmentDepth > 0 || (wudingActive && !isSameType))
        {
            if (card.Type is CardType.Status or CardType.Curse or CardType.Quest)
            {
                __result = false;
                return false;
            }

            if (!__instance.CanEnchantCardType(card.Type))
            {
                __result = false;
                return false;
            }

            if (card.Pile?.Type == PileType.Deck && card.Keywords.Contains(CardKeyword.Eternal))
            {
                __result = false;
                return false;
            }

            __result = true;
            return false;
        }

        return true;
    }

    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.CanEnchant), [typeof(CardModel)])]
    [HarmonyFinalizer]
    public static void EnchantmentCanEnchantFinalizer(bool __state)
    {
        if (__state)
            _allowWudingStacking = false;
    }

    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.CanEnchant), [typeof(CardModel)])]
    [HarmonyPostfix]
    public static void EnchantmentCanEnchantPostfix(EnchantmentModel __instance, CardModel card, ref bool __result)
    {
        if (!__result || card.Enchantment == null || _allowReplaceEnchantmentDepth > 0)
            return;

        if (card.Owner?.Character is not Character.ShoreMagician)
            return;

        if (WudingRiverPower.IsActiveFor(card.Owner))
        {
            var isSameType = card.Enchantment.GetType() == __instance.GetType();
            if (!isSameType || WudingRiverPower.IsNumericStackable(__instance))
                return;
        }

        __result = false;
    }

    // 0a1. Under WudingRiver, handle different-type overwrite cleanly even for vanilla CardCmd.Enchant calls,
    // and re-enable Disabled one-shot numeric enchantments (Swift/Sown/Vigorous) when stacked.
    [HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Enchant), [typeof(EnchantmentModel), typeof(CardModel), typeof(decimal)])]
    [HarmonyPrefix]
    public static void CardCmdEnchantPrefix(EnchantmentModel enchantment, CardModel card)
    {
        if (card.Enchantment == null || card.Enchantment.GetType() == enchantment.GetType())
            return;

        if (WudingRiverPower.IsActiveFor(card.Owner) && enchantment.CanEnchant(card))
            CardCmd.ClearEnchantment(card);
    }

    [HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Enchant), [typeof(EnchantmentModel), typeof(CardModel), typeof(decimal)])]
    [HarmonyPostfix]
    public static void CardCmdEnchantPostfix(CardModel card, EnchantmentModel? __result)
    {
        if (__result == null)
            return;

        if (__result.Status == EnchantmentStatus.Disabled
            && WudingRiverPower.IsNumericStackable(__result)
            && WudingRiverPower.IsActiveFor(card.Owner))
        {
            __result.Status = EnchantmentStatus.Normal;
        }

        if (__result is Adroit adroit && card.IsInCombat)
            adroit.RecalculateValues();
    }

    // 0a2. NutritiousSoup also enchants ManaStrike (Common) in deck with TezcatarasEmber.
    [HarmonyPatch(typeof(NutritiousSoup), nameof(NutritiousSoup.AfterObtained))]
    [HarmonyPostfix]
    public static void NutritiousSoupAfterObtainedPostfix(NutritiousSoup __instance)
    {
        var ember = ModelDb.Enchantment<TezcatarasEmber>();
        foreach (var card in PileType.Deck.GetPile(__instance.Owner).Cards.ToList())
        {
            if (card is not ManaStrike || !ember.CanEnchant(card))
                continue;

            CardCmd.Enchant<TezcatarasEmber>(card, 1m);
            var vfx = NCardEnchantVfx.Create(card);
            if (vfx != null)
                NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(vfx);
        }
    }

    // 0b. Moon of Lighthouse (cursed) & All Rivers to the Sea (rarity climb) redirect in-combat random transforms.
    [HarmonyPatch(typeof(CardFactory), nameof(CardFactory.CreateRandomCardForTransform), [typeof(CardModel), typeof(bool), typeof(Rng)])]
    [HarmonyPrefix]
    public static bool CreateRandomCardForTransformPrefix(CardModel original, bool isInCombat, Rng rng, ref CardModel __result)
    {
        if (!isInCombat)
            return true;

        var custom = MoonOfLighthousePower.TryCreateCurseReplacement(original, rng)
            ?? AllRiversToSeaPower.TryCreateReplacement(original, rng);
        if (custom == null)
            return true;

        __result = custom;
        return false;
    }

    // 0c. WudingRiver upgrades (if upgraded) and grants a random enchantment to every card transformed in combat.
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.AfterThisWasTransformedFrom))]
    [HarmonyPostfix]
    public static void AfterThisWasTransformedFromPostfix(CardModel __instance)
    {
        WudingRiverPower.OnCardTransformedInCombat(__instance);
    }

    // 0d. Dispatch ShoreMagicianHook.OnCardTransformed for non-ShoreMagicianCmd in-combat transforms (vanilla / cross-class).
    [HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Transform), [typeof(CardModel), typeof(CardModel), typeof(CardPreviewStyle)])]
    [HarmonyPostfix]
    public static void CardCmdTransformPostfix(CardModel original, ref Task<CardPileAddResult?> __result)
    {
        if (ShoreMagicianCmd.IsHandlingTransform || !original.IsInCombat || original.Owner == null)
            return;

        __result = WrapVanillaTransformAsync(__result, original);
    }

    private static async Task<CardPileAddResult?> WrapVanillaTransformAsync(Task<CardPileAddResult?> inner, CardModel original)
    {
        var result = await inner;
        if (result?.cardAdded is { } replacement && original.Owner != null)
        {
            await ShoreMagicianHook.OnCardTransformed(
                new BlockingPlayerChoiceContext(),
                original.Owner,
                original,
                replacement,
                null);
        }

        return result;
    }

    // 1. DisplayAmount on card enchantment badge shows final effective value.
    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.DisplayAmount), MethodType.Getter)]
    [HarmonyPostfix]
    public static void DisplayAmountPostfix(EnchantmentModel __instance, ref int __result)
    {
        if (__instance.IsBoostedEnchantment() && __instance.HasCard && __instance.Card.IsInCombat)
            __result = __instance.GetEffectiveAmount();
    }

    // 2. Tooltip description shows final effective {Amount} (and Adroit {Block}).
    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.DynamicDescription), MethodType.Getter)]
    [HarmonyPrefix]
    public static void DynamicDescriptionPrefix(EnchantmentModel __instance)
    {
        if (__instance is Adroit && __instance.HasCard && __instance.Card.IsInCombat)
            __instance.RecalculateValues();
    }

    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.DynamicDescription), MethodType.Getter)]
    [HarmonyPostfix]
    public static void DynamicDescriptionPostfix(EnchantmentModel __instance, LocString __result)
    {
        if (__instance.IsBoostedEnchantment() && __instance.HasCard && __instance.Card.IsInCombat)
            __result.Add("Amount", (decimal)__instance.GetEffectiveAmount());
    }

    // 3. Extra card text on the card face shows final effective {Amount} (and Adroit {Block}).
    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.DynamicExtraCardText), MethodType.Getter)]
    [HarmonyPrefix]
    public static void DynamicExtraCardTextPrefix(EnchantmentModel __instance)
    {
        if (__instance is Adroit && __instance.HasCard && __instance.Card.IsInCombat)
            __instance.RecalculateValues();
    }

    [HarmonyPatch(typeof(EnchantmentModel), nameof(EnchantmentModel.DynamicExtraCardText), MethodType.Getter)]
    [HarmonyPostfix]
    public static void DynamicExtraCardTextPostfix(EnchantmentModel __instance, LocString? __result)
    {
        if (__result != null && __instance.IsBoostedEnchantment() && __instance.HasCard && __instance.Card.IsInCombat)
            __result.Add("Amount", (decimal)__instance.GetEffectiveAmount());
    }

    // 4. Sharp adds effective amount to powered attacks.
    [HarmonyPatch(typeof(Sharp), nameof(Sharp.EnchantDamageAdditive))]
    [HarmonyPostfix]
    public static void SharpDamagePostfix(Sharp __instance, ValueProp props, ref decimal __result)
    {
        if (props.IsPoweredAttack() && __instance.HasCard && __instance.Card.IsInCombat)
            __result = __instance.GetEffectiveAmount();
    }

    // 5. Nimble adds effective amount to block.
    [HarmonyPatch(typeof(Nimble), nameof(Nimble.EnchantBlockAdditive))]
    [HarmonyPostfix]
    public static void NimbleBlockPostfix(Nimble __instance, ref decimal __result)
    {
        if (__instance.HasCard && __instance.Card.IsInCombat)
            __result = __instance.GetEffectiveAmount();
    }

    // 6. Adroit computes BlockVar = effective amount both for preview and execution.
    [HarmonyPatch(typeof(Adroit), nameof(Adroit.RecalculateValues))]
    [HarmonyPostfix]
    public static void AdroitRecalculatePostfix(Adroit __instance)
    {
        if (__instance.HasCard && __instance.Card.IsInCombat)
            __instance.DynamicVars.Block.BaseValue = __instance.GetEffectiveAmount();
    }

    [HarmonyPatch(typeof(Adroit), nameof(Adroit.OnPlay))]
    [HarmonyPrefix]
    public static void AdroitOnPlayPrefix(Adroit __instance)
    {
        if (__instance.HasCard && __instance.Card.IsInCombat)
            __instance.RecalculateValues();
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.UpdateDynamicVarPreview))]
    [HarmonyPostfix]
    public static void CardPreviewPostfix(CardModel __instance)
    {
        if (__instance.Enchantment is Adroit adroit && __instance.IsInCombat)
            adroit.RecalculateValues();
    }

    // 7. Swift draws effective amount on first play.
    [HarmonyPatch(typeof(Swift), nameof(Swift.OnPlay))]
    [HarmonyPrefix]
    public static bool SwiftOnPlayPrefix(Swift __instance, PlayerChoiceContext choiceContext, ref Task __result)
    {
        if (!__instance.HasCard || !__instance.Card.IsInCombat)
            return true;

        __result = ExecuteSwiftAsync(__instance, choiceContext);
        return false;
    }

    private static async Task ExecuteSwiftAsync(Swift swift, PlayerChoiceContext choiceContext)
    {
        if (swift.Status != EnchantmentStatus.Normal)
            return;

        var drawCount = swift.GetEffectiveAmount();
        if (drawCount > 0)
            await CardPileCmd.Draw(choiceContext, drawCount, swift.Card.Owner);
        swift.Status = EnchantmentStatus.Disabled;
    }

    // 7b. Sown grants effective energy (affected by MagicPower and doubling) on first play.
    // Note: vanilla Sown.AfterCardPlayed disables Sown after the card finishes playing.
    [HarmonyPatch(typeof(Sown), nameof(Sown.OnPlay))]
    [HarmonyPrefix]
    public static bool SownOnPlayPrefix(Sown __instance, ref Task __result)
    {
        if (!__instance.HasCard || !__instance.Card.IsInCombat)
            return true;

        __result = ExecuteSownAsync(__instance);
        return false;
    }

    private static async Task ExecuteSownAsync(Sown sown)
    {
        if (sown.Status != EnchantmentStatus.Normal)
            return;

        var energyAmount = sown.GetEffectiveAmount();
        if (energyAmount > 0)
            await PlayerCmd.GainEnergy(energyAmount, sown.Card.Owner);
    }

    // 8. Stack-safe per-play snapshot for enchantment presence and doubling, avoiding self-consumption,
    // double-burn between PreemptiveVoice and DoubleNextEnchantmentPower, and nested AutoPlay clobber.
    [HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
    [HarmonyPrefix]
    public static void CardOnPlayWrapperPrefix(CardModel __instance, out ShoreMagicianModel.CardPlaySnapshot? __state)
    {
        __state = null;
        if (__instance.Owner == null || !__instance.IsInCombat)
            return;

        var tracker = ShoreMagicianModel.GetTracker(__instance.Owner);
        var wasEnchanted = __instance.Enchantment != null;
        var isBoosted = __instance.Enchantment.IsBoostedEnchantment();

        var usePreemptive = isBoosted
            && __instance.Owner.Creature.HasPower<PreemptiveVoicePower>()
            && tracker.EnchantedCardsPlayedThisTurn == 0
            && !tracker.PreemptiveClaimedInFlight;

        var doublePower = __instance.Owner.Creature.GetPower<DoubleNextEnchantmentPower>();
        var useDoubleNext = isBoosted
            && !usePreemptive
            && doublePower != null
            && doublePower.Amount > tracker.DoubleNextReservedCount;

        if (usePreemptive)
            tracker.PreemptiveClaimedInFlight = true;
        if (useDoubleNext)
            tracker.DoubleNextReservedCount++;

        var snapshot = new ShoreMagicianModel.CardPlaySnapshot(
            __instance,
            wasEnchanted,
            usePreemptive || useDoubleNext,
            usePreemptive,
            useDoubleNext);
        tracker.ActivePlays.Add(snapshot);
        __state = snapshot;
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.OnPlayWrapper))]
    [HarmonyPostfix]
    public static void CardOnPlayWrapperPostfix(
        CardModel __instance,
        ShoreMagicianModel.CardPlaySnapshot? __state,
        ref Task __result)
    {
        if (__state == null)
            return;

        __result = WrapCleanup(__result, __instance, __state);
    }

    private static async Task WrapCleanup(Task inner, CardModel card, ShoreMagicianModel.CardPlaySnapshot snapshot)
    {
        try
        {
            await inner;
        }
        finally
        {
            if (card.Owner != null)
            {
                var tracker = ShoreMagicianModel.GetTracker(card.Owner);
                tracker.ActivePlays.Remove(snapshot);

                if (snapshot.ClaimedPreemptiveVoice)
                    tracker.PreemptiveClaimedInFlight = false;

                if (snapshot.ClaimedDoubleNext)
                {
                    tracker.DoubleNextReservedCount = Math.Max(0, tracker.DoubleNextReservedCount - 1);
                    var doublePower = card.Owner.Creature.GetPower<DoubleNextEnchantmentPower>();
                    if (doublePower != null)
                        await PowerCmd.Decrement(doublePower);
                }

                if (snapshot.WasEnchantedAtStart && card is not PreemptiveVoice)
                    tracker.EnchantedCardsPlayedThisTurn++;
            }
        }
    }

    // 9. Crouching Tiger doubles Vulnerable/Weak modifiers on Slowed enemies, matching Necrobinder's Debilitate math.
    [HarmonyPatch(typeof(VulnerablePower), nameof(VulnerablePower.ModifyDamageMultiplicative))]
    [HarmonyPostfix]
    public static void CrouchingTigerVulnerablePostfix(
        VulnerablePower __instance,
        Creature? target,
        ValueProp props,
        Creature? dealer,
        ref decimal __result)
    {
        if (!props.IsPoweredAttack()
            || target != __instance.Owner
            || !__instance.Owner.IsEnemy
            || !__instance.Owner.HasPower<SlowPower>()
            || !HasOpponentCrouchingTiger(dealer))
            return;

        __result += __result - 1m;
    }

    [HarmonyPatch(typeof(WeakPower), nameof(WeakPower.ModifyDamageMultiplicative))]
    [HarmonyPostfix]
    public static void CrouchingTigerWeakPostfix(
        WeakPower __instance,
        ValueProp props,
        Creature? dealer,
        ref decimal __result)
    {
        if (!props.IsPoweredAttack()
            || dealer != __instance.Owner
            || !__instance.Owner.IsEnemy
            || !__instance.Owner.HasPower<SlowPower>()
            || !(__instance.Owner.CombatState?.PlayerCreatures.Any(c => c.HasPower<CrouchingTigerPower>()) ?? false))
            return;

        __result = Math.Max(0m, __result - (1m - __result));
    }

    private static bool HasOpponentCrouchingTiger(Creature? dealer)
    {
        if (dealer == null)
            return false;

        if (dealer.HasPower<CrouchingTigerPower>() || dealer.PetOwner?.Creature.HasPower<CrouchingTigerPower>() == true)
            return true;

        return dealer.CombatState?.GetTeammatesOf(dealer).Any(c => c.HasPower<CrouchingTigerPower>()) ?? false;
    }
}
