using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ShoreMagician.Code.Events;

public interface IModifyEnchantmentAmount
{
    int ModifyEnchantmentAmount(Player player, EnchantmentModel enchantment, CardModel targetCard, int currentAmount);
}

public interface IOnCardEnchanted
{
    Task OnCardEnchanted(PlayerChoiceContext ctx, Player player, CardModel targetCard, EnchantmentModel enchantment, int amount, CardModel? source);
}

public interface IOnEnchantmentCleared
{
    Task OnEnchantmentCleared(PlayerChoiceContext ctx, Player player, CardModel targetCard, CardModel? source);
}

public interface IOnCardTransformed
{
    Task OnCardTransformed(PlayerChoiceContext ctx, Player player, CardModel originalCard, CardModel? newCard, CardModel? source);
}
