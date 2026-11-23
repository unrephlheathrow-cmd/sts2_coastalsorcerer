using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Extensions;

public static class PlayerExtensions
{
    public static IReadOnlyList<CardModel> GetHand(this Player player) =>
        PileType.Hand.GetPile(player).Cards;

    public static IReadOnlyList<CardModel> GetDiscardPile(this Player player) =>
        PileType.Discard.GetPile(player).Cards;

    public static IReadOnlyList<CardModel> GetDrawPile(this Player player) =>
        PileType.Draw.GetPile(player).Cards;

    public static IReadOnlyList<CardModel> GetExhaustPile(this Player player) =>
        PileType.Exhaust.GetPile(player).Cards;

    public static IReadOnlyList<CardModel> GetDeckPile(this Player player) =>
        PileType.Deck.GetPile(player).Cards;

    public static IReadOnlyList<Creature> GetAllOpponents(this Player player) =>
        player.Creature.CombatState?
            .GetOpponentsOf(player.Creature)
            .Where(c => c.IsHittable)
            .ToArray()
        ?? [];

    public static int GetMagicPowerAmount(this Player player)
    {
        var perm = player.Creature.GetPowerAmount<MagicPower>();
        var temp = player.Creature.GetPowerAmount<TempMagicPower>();
        return Math.Max(0, perm + temp);
    }
}
