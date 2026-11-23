using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Core;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Extensions;

public static class EnchantmentExtensions
{
    public static bool IsBoostedEnchantment(this EnchantmentModel? enchantment) =>
        enchantment is Sharp or Nimble or Adroit or Swift or Sown;

    public static bool IsEnchantmentDoubled(this CardModel card)
    {
        if (card.Enchantment == null || !card.Enchantment.IsBoostedEnchantment() || !card.IsInCombat || card.Owner == null)
            return false;

        if (ShoreMagicianModel.FindActivePlay(card) is { } activePlay)
            return activePlay.IsDoubled;

        // Only preview doubling while in hand when the card is not currently resolving.
        if (card.Pile?.Type == PileType.Hand)
        {
            var tracker = ShoreMagicianModel.GetTracker(card.Owner);
            var doublePower = card.Owner.Creature.GetPower<DoubleNextEnchantmentPower>();
            if (doublePower != null && doublePower.Amount > tracker.DoubleNextReservedCount)
                return true;

            if (card.Owner.Creature.HasPower<PreemptiveVoicePower>()
                && tracker.EnchantedCardsPlayedThisTurn == 0
                && !tracker.PreemptiveClaimedInFlight)
                return true;
        }

        return false;
    }

    public static int GetEffectiveAmount(this EnchantmentModel enchantment)
    {
        if (!enchantment.IsBoostedEnchantment() || !enchantment.HasCard || enchantment.Card.Owner == null || !enchantment.Card.IsInCombat)
            return enchantment.Amount;

        var magic = enchantment.Card.Owner.GetMagicPowerAmount();
        var total = Math.Max(0, enchantment.Amount + magic);
        if (enchantment.Card.IsEnchantmentDoubled())
            total *= 2;
        return total;
    }
}
