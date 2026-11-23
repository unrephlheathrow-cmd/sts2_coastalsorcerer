using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using ShoreMagician.Code.Events;
using ShoreMagician.Code.Patches;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Commands;

public static class ShoreMagicianCmd
{
    private static readonly Color LightBlueTransformTint = new(0.50f, 0.85f, 1.0f, 1.0f);

    [ThreadStatic]
    private static int _handlingTransformDepth;

    public static bool IsHandlingTransform => _handlingTransformDepth > 0;

    public static bool CanEnchantWith<T>(CardModel card) where T : EnchantmentModel =>
        ModelDb.Enchantment<T>().CanEnchant(card);

    public static bool CanReplaceEnchantmentWith<T>(CardModel card) where T : EnchantmentModel =>
        EnchantmentMagicPowerPatch.RunAllowReplaceEnchantment(() => ModelDb.Enchantment<T>().CanEnchant(card));

    public static async Task<bool> EnchantCard<T>(
        PlayerChoiceContext ctx,
        Player sourcePlayer,
        CardModel targetCard,
        int baseAmount,
        CardModel? cardSource) where T : EnchantmentModel
    {
        if (baseAmount <= 0 || !CanEnchantWith<T>(targetCard))
            return false;

        var clearDifferentUnderWuding = targetCard.Enchantment != null
            && targetCard.Enchantment.GetType() != typeof(T)
            && WudingRiverPower.IsActiveFor(targetCard.Owner);

        return await ApplyEnchantmentCore<T>(ctx, sourcePlayer, targetCard, baseAmount, cardSource, clearExisting: clearDifferentUnderWuding);
    }

    // Modifying/replacing an enchantment on a card counts as a single card-change event.
    public static async Task<bool> ReplaceEnchantment<T>(
        PlayerChoiceContext ctx,
        Player sourcePlayer,
        CardModel targetCard,
        int baseAmount,
        CardModel? cardSource) where T : EnchantmentModel
    {
        if (baseAmount <= 0 || !CanReplaceEnchantmentWith<T>(targetCard))
            return false;

        return await ApplyEnchantmentCore<T>(ctx, sourcePlayer, targetCard, baseAmount, cardSource, clearExisting: targetCard.Enchantment != null);
    }

    public static async Task<bool> DoubleSharpEnchantment(
        PlayerChoiceContext ctx,
        Player sourcePlayer,
        CardModel targetCard,
        CardModel? cardSource)
    {
        if (targetCard.Enchantment is not Sharp sharp || sharp.Amount <= 0)
            return false;

        var delta = Math.Clamp(sharp.Amount, 1, 9999 - sharp.Amount);
        if (delta <= 0)
            return false;

        sharp.Amount += delta;

        await PlayLightBlueTransformVfxAsync(targetCard);
        await ShoreMagicianHook.OnCardEnchanted(ctx, sourcePlayer, targetCard, sharp, sharp.Amount, cardSource);

        if (targetCard.IsInCombat && targetCard.Pile?.Type.IsCombatPile() == true && sourcePlayer.Creature.CombatState != null)
            await Hook.AfterCardGeneratedForCombat(sourcePlayer.Creature.CombatState, targetCard, sourcePlayer);

        return true;
    }

    public static async Task<CardModel?> TransformCardTo<T>(
        PlayerChoiceContext ctx,
        Player owner,
        CardModel card,
        CardModel? cardSource) where T : CardModel
    {
        if (!card.IsTransformable)
            return null;

        Task<CardPileAddResult?> task;
        _handlingTransformDepth++;
        try
        {
            task = CardCmd.TransformTo<T>(card);
        }
        finally
        {
            _handlingTransformDepth--;
        }

        var result = await task;
        var replacement = result?.cardAdded;
        await ShoreMagicianHook.OnCardTransformed(ctx, owner, card, replacement, cardSource);
        return replacement;
    }

    public static async Task<IReadOnlyList<CardPileAddResult>> AddGeneratedCardsToCombat<TCard>(
        Player owner,
        int count,
        PileType pileType,
        CardPilePosition position,
        bool upgrade = false) where TCard : CardModel
    {
        if (count <= 0 || owner.Creature.CombatState is not { } cs)
            return [];

        var created = new List<CardModel>(count);
        for (var i = 0; i < count; i++)
        {
            var card = cs.CreateCard<TCard>(owner);
            if (upgrade && card.IsUpgradable)
                CardCmd.Upgrade(card);
            created.Add(card);
        }

        var results = (await CardPileCmd.AddGeneratedCardsToCombat(created, pileType, owner, position)).ToList();
        if (results.Count > 0)
            CardCmd.PreviewCardPileAdd(results);
        return results;
    }

    private static async Task<bool> ApplyEnchantmentCore<T>(
        PlayerChoiceContext ctx,
        Player sourcePlayer,
        CardModel targetCard,
        int baseAmount,
        CardModel? cardSource,
        bool clearExisting) where T : EnchantmentModel
    {
        var template = ModelDb.Enchantment<T>();
        var finalAmount = ShoreMagicianHook.ModifyEnchantmentAmount(
            sourcePlayer.Creature.CombatState,
            sourcePlayer,
            template,
            targetCard,
            baseAmount);

        if (finalAmount <= 0)
            return false;

        if (clearExisting && targetCard.Enchantment != null)
            CardCmd.ClearEnchantment(targetCard);

        var applied = CardCmd.Enchant<T>(targetCard, finalAmount);
        await PlayLightBlueTransformVfxAsync(targetCard);
        await ShoreMagicianHook.OnCardEnchanted(ctx, sourcePlayer, targetCard, applied!, finalAmount, cardSource);

        // Treat enchanting as a transformation-equivalent card creation for combat hooks (e.g. ArsenalPower).
        if (targetCard.IsInCombat && targetCard.Pile?.Type.IsCombatPile() == true && sourcePlayer.Creature.CombatState != null)
            await Hook.AfterCardGeneratedForCombat(sourcePlayer.Creature.CombatState, targetCard, sourcePlayer);

        return true;
    }

    private static async Task PlayLightBlueTransformVfxAsync(CardModel card)
    {
        if (TestMode.IsOn)
            return;

        var cardNode = NCard.FindOnTable(card, card.Pile?.Type);
        if (cardNode == null)
            return;

        SfxCmd.Play("event:/sfx/ui/cards/card_transform");
        var shineVfx = NCardTransformShineVfx.Create(cardNode, card, Array.Empty<RelicModel>());
        if (shineVfx == null)
            return;

        shineVfx.Modulate = LightBlueTransformTint;
        await shineVfx.PlayAnimation(true);
    }

    public static async Task<bool> ClearEnchantment(
        PlayerChoiceContext ctx,
        Player sourcePlayer,
        CardModel targetCard,
        CardModel? cardSource)
    {
        if (targetCard.Enchantment == null)
            return false;

        CardCmd.ClearEnchantment(targetCard);
        await ShoreMagicianHook.OnEnchantmentCleared(ctx, sourcePlayer, targetCard, cardSource);
        return true;
    }

    public static async Task<bool> ClearUpgradeOrEnchantment(
        PlayerChoiceContext ctx,
        Player sourcePlayer,
        CardModel targetCard,
        CardModel? cardSource)
    {
        if (targetCard.Enchantment != null)
            return await ClearEnchantment(ctx, sourcePlayer, targetCard, cardSource);

        if (targetCard.IsUpgraded)
        {
            CardCmd.Downgrade(targetCard);
            return true;
        }

        return false;
    }

    public static async Task<CardModel?> TransformCard(
        PlayerChoiceContext ctx,
        Player owner,
        CardModel card,
        CardModel? cardSource)
    {
        if (!card.IsTransformable)
            return null;

        Task<CardPileAddResult> task;
        _handlingTransformDepth++;
        try
        {
            task = CardCmd.TransformToRandom(card, owner.RunState.Rng.CombatCardGeneration);
        }
        finally
        {
            _handlingTransformDepth--;
        }

        var result = await task;
        var replacement = result.cardAdded;
        await ShoreMagicianHook.OnCardTransformed(ctx, owner, card, replacement, cardSource);
        return replacement;
    }

    public static async Task<CardModel?> TransformToRareSameType(
        PlayerChoiceContext ctx,
        Player owner,
        CardModel card,
        CardModel? cardSource)
    {
        if (!card.IsTransformable)
            return null;

        var rng = owner.RunState.Rng.CombatCardGeneration;
        if (MoonOfLighthousePower.TryCreateCurseReplacement(card, rng) is { } lighthouseCard)
        {
            Task<CardPileAddResult?> task;
            _handlingTransformDepth++;
            try
            {
                task = CardCmd.Transform(card, lighthouseCard);
            }
            finally
            {
                _handlingTransformDepth--;
            }

            var res = await task;
            var rep = res?.cardAdded;
            await ShoreMagicianHook.OnCardTransformed(ctx, owner, card, rep, cardSource);
            return rep;
        }

        var sameTypeRareOptions = CardFactory.GetDefaultTransformationOptions(card, true)
            .Where(c => c.Rarity == CardRarity.Rare && c.Type == card.Type && c.Id != card.Id)
            .ToList();

        if (sameTypeRareOptions.Count == 0)
        {
            sameTypeRareOptions = owner.Character.CardPool.AllCards
                .Where(c => c.Rarity == CardRarity.Rare && c.Type == card.Type && c.Id != card.Id)
                .ToList();
        }

        CardModel? replacement;
        if (sameTypeRareOptions.Count > 0)
        {
            var generated = CardFactory.CreateRandomCardForTransform(card, sameTypeRareOptions, true, rng);
            Task<CardPileAddResult?> task;
            _handlingTransformDepth++;
            try
            {
                task = CardCmd.Transform(card, generated);
            }
            finally
            {
                _handlingTransformDepth--;
            }

            var res = await task;
            replacement = res?.cardAdded;
        }
        else
        {
            Task<CardPileAddResult> task;
            _handlingTransformDepth++;
            try
            {
                task = CardCmd.TransformToRandom(card, rng);
            }
            finally
            {
                _handlingTransformDepth--;
            }

            var res = await task;
            replacement = res.cardAdded;
        }

        await ShoreMagicianHook.OnCardTransformed(ctx, owner, card, replacement, cardSource);
        return replacement;
    }

    public static async Task<CardModel?> TransformToZeroCostCard(
        PlayerChoiceContext ctx,
        Player owner,
        CardModel card,
        CardModel? cardSource)
    {
        if (!card.IsTransformable)
            return null;

        var rng = owner.RunState.Rng.CombatCardGeneration;
        static bool IsZeroCostCard(CardModel c, ModelId excludeId) =>
            c.EnergyCost.Canonical == 0
            && !c.EnergyCost.CostsX
            && c.CanonicalStarCost <= 0
            && c.CanBeGeneratedInCombat
            && c.Id != excludeId;

        var zeroCostOptions = CardFactory.GetDefaultTransformationOptions(card, true)
            .Where(c => IsZeroCostCard(c, card.Id))
            .ToList();

        if (zeroCostOptions.Count == 0)
        {
            var unlockedCharPool = owner.Character.CardPool
                .GetUnlockedCards(owner.UnlockState, owner.RunState.CardMultiplayerConstraint)
                .Where(c => IsZeroCostCard(c, card.Id));
            zeroCostOptions = CardFactory.FilterForRun(owner.RunState, unlockedCharPool).ToList();
        }

        if (zeroCostOptions.Count == 0)
        {
            var allCharZeroCost = ModelDb.AllCharacterCardPools
                .SelectMany(p => p.GetUnlockedCards(owner.UnlockState, owner.RunState.CardMultiplayerConstraint))
                .Where(c => IsZeroCostCard(c, card.Id))
                .DistinctBy(c => c.Id);
            zeroCostOptions = CardFactory.FilterForRun(owner.RunState, allCharZeroCost).ToList();
        }

        CardModel? replacement;
        if (zeroCostOptions.Count > 0)
        {
            var generated = CardFactory.CreateRandomCardForTransform(card, zeroCostOptions, true, rng);
            Task<CardPileAddResult?> task;
            _handlingTransformDepth++;
            try
            {
                task = CardCmd.Transform(card, generated);
            }
            finally
            {
                _handlingTransformDepth--;
            }

            var res = await task;
            replacement = res?.cardAdded;
        }
        else
        {
            Task<CardPileAddResult> task;
            _handlingTransformDepth++;
            try
            {
                task = CardCmd.TransformToRandom(card, rng);
            }
            finally
            {
                _handlingTransformDepth--;
            }

            var res = await task;
            replacement = res.cardAdded;
        }

        await ShoreMagicianHook.OnCardTransformed(ctx, owner, card, replacement, cardSource);
        return replacement;
    }
}
