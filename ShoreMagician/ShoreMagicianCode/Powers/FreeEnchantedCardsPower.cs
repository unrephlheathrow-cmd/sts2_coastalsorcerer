using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Core;

namespace ShoreMagician.Code.Powers;

public sealed class FreeEnchantedCardsPower : ShoreMagicianPowerModel
{
    public CardModel? IgnoreCurrentPlayCard { get; set; }

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    // Late cost override ensures "0 cost" wins over +1 cost penalties (ExhaustingStrike, WeirdWave, BorrowedTime).
    public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner?.Creature != Owner || card.Enchantment == null || card.EnergyCost.CostsX || Amount <= 0)
        {
            modifiedCost = originalCost;
            return false;
        }

        modifiedCost = 0m;
        return true;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner?.Creature != Owner)
            return;

        if (ReferenceEquals(cardPlay.Card, IgnoreCurrentPlayCard))
        {
            IgnoreCurrentPlayCard = null;
            return;
        }

        if (cardPlay.IsAutoPlay || cardPlay.Card.EnergyCost.CostsX)
            return;

        if (ShoreMagicianModel.WasEnchantedAtPlayStart(cardPlay.Card))
            await PowerCmd.Decrement(this);
    }
}
