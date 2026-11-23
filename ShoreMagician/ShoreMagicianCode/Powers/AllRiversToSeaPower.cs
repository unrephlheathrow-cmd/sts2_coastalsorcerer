using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class AllRiversToSeaPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public static bool IsActiveFor(Player? player) =>
        player?.Creature.HasPower<AllRiversToSeaPower>() == true
        || player?.Creature.HasPower<AllRiversToSeaUpgradedPower>() == true;

    public static bool IsUpgradedActiveFor(Player? player) =>
        player?.Creature.HasPower<AllRiversToSeaUpgradedPower>() == true;

    public static CardModel? TryCreateReplacement(CardModel original, Rng rng)
    {
        if (original.CombatState == null || original.Owner is not { } owner || !IsActiveFor(owner))
            return null;

        var result = original.Rarity switch
        {
            CardRarity.Basic or CardRarity.Common => CreateTierUpgradeReplacement(original, owner, CardRarity.Uncommon, rng),
            CardRarity.Uncommon => CreateTierUpgradeReplacement(original, owner, CardRarity.Rare, rng),
            CardRarity.Rare => CreateRareTranscendReplacement(original, owner, rng),
            _ => null
        };

        if (result != null)
        {
            PowerModel? activePower = owner.Creature.GetPower<AllRiversToSeaUpgradedPower>()
                ?? (PowerModel?)owner.Creature.GetPower<AllRiversToSeaPower>();
            activePower?.Flash();
        }

        return result;
    }

    private static CardModel? CreateTierUpgradeReplacement(CardModel original, Player owner, CardRarity targetRarity, Rng rng)
    {
        var primaryPool = !original.Pool.IsColorless ? original.Pool : owner.Character.CardPool;
        var candidates = QueryUnlockedRarity(primaryPool, owner, targetRarity, original);

        if (candidates.Count == 0 && primaryPool != owner.Character.CardPool)
            candidates = QueryUnlockedRarity(owner.Character.CardPool, owner, targetRarity, original);

        if (candidates.Count == 0)
        {
            candidates = ModelDb.AllCharacterCardPools
                .SelectMany(p => QueryUnlockedRarity(p, owner, targetRarity, original))
                .DistinctBy(c => c.Id)
                .ToList();
        }

        if (candidates.Count == 0)
            return null;

        var scope = original.CardScope ?? owner.RunState;
        var chosen = rng.NextItem(candidates);
        return chosen != null ? scope.CreateCard(chosen, owner) : null;
    }

    private static List<CardModel> QueryUnlockedRarity(CardPoolModel pool, Player owner, CardRarity targetRarity, CardModel excluded)
    {
        var raw = pool.GetUnlockedCards(owner.UnlockState, owner.RunState.CardMultiplayerConstraint)
            .Where(c => c.Rarity == targetRarity && c.CanBeGeneratedInCombat && c.Id != excluded.Id);
        return CardFactory.FilterForRun(owner.RunState, raw).ToList();
    }

    private static CardModel? CreateRareTranscendReplacement(CardModel original, Player owner, Rng rng)
    {
        var options = new List<(CardModel Template, bool IsOtherCharRare)>();

        foreach (var tarot in TarotCards.All)
        {
            if (tarot.Id != original.Id)
                options.Add((tarot, false));
        }

        var otherPoolCards = ModelDb.AllCharacterCardPools
            .Where(p => p != owner.Character.CardPool)
            .SelectMany(p => p.GetUnlockedCards(owner.UnlockState, owner.RunState.CardMultiplayerConstraint))
            .Where(c => c.Rarity == CardRarity.Rare
                && c.EnergyCost.Canonical == 0
                && !c.EnergyCost.CostsX
                && c.CanonicalStarCost <= 0
                && c.CanBeGeneratedInCombat
                && c.Id != original.Id);

        foreach (var card in CardFactory.FilterForRun(owner.RunState, otherPoolCards).DistinctBy(c => c.Id))
            options.Add((card, true));

        if (options.Count == 0)
            return null;

        var chosen = rng.NextItem(options);
        if (chosen.Template == null)
            return null;

        var scope = original.CardScope ?? owner.RunState;
        var created = scope.CreateCard(chosen.Template, owner);
        if (chosen.IsOtherCharRare && IsUpgradedActiveFor(owner))
            CardCmd.ApplyKeyword(created, CardKeyword.Ethereal);

        return created;
    }
}

public sealed class AllRiversToSeaUpgradedPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
}
