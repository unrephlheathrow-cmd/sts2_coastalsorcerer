using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class OceanCurrentPower : ShoreMagicianPowerModel
{
    private const int MaxDrawsPerChain = 50;
    private bool _isDrawing;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (_isDrawing || card.Owner?.Creature != Owner || card.Type != CardType.Curse || CombatManager.Instance.IsEnding)
            return;

        _isDrawing = true;
        try
        {
            Flash();
            var player = card.Owner;
            var hand = PileType.Hand.GetPile(player);
            var draws = 0;

            while (!CombatManager.Instance.IsEnding
                && hand.Cards.Count < CardPile.MaxCardsInHand
                && draws < MaxDrawsPerChain)
            {
                var drawn = await CardPileCmd.Draw(choiceContext, player);
                if (drawn == null)
                    break;

                draws++;

                // Drawing another Curse starts another wave; drawing an enchanted card continues;
                // drawing an unenchanted non-Curse card stops the chain.
                if (drawn.Type == CardType.Curse)
                {
                    Flash();
                    continue;
                }

                if (drawn.Enchantment == null)
                    break;
            }
        }
        finally
        {
            _isDrawing = false;
        }
    }
}
