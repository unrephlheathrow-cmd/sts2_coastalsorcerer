using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ShoreMagician.Code.Events;

public static class ShoreMagicianHook
{
    public static int ModifyEnchantmentAmount(ICombatState? cs, Player player, EnchantmentModel enchantment, CardModel targetCard, int baseAmount)
    {
        if (cs == null) return baseAmount;
        return HookUtils.Aggregate<IModifyEnchantmentAmount, int>(cs, baseAmount,
            (m, current) => m.ModifyEnchantmentAmount(player, enchantment, targetCard, current));
    }

    public static Task OnCardEnchanted(PlayerChoiceContext ctx, Player player, CardModel targetCard, EnchantmentModel enchantment, int amount, CardModel? source)
    {
        var cs = player.Creature.CombatState;
        if (cs == null) return Task.CompletedTask;
        return HookUtils.Dispatch<IOnCardEnchanted>(cs, ctx,
            m => m.OnCardEnchanted(ctx, player, targetCard, enchantment, amount, source));
    }

    public static Task OnEnchantmentCleared(PlayerChoiceContext ctx, Player player, CardModel targetCard, CardModel? source)
    {
        var cs = player.Creature.CombatState;
        if (cs == null) return Task.CompletedTask;
        return HookUtils.Dispatch<IOnEnchantmentCleared>(cs, ctx,
            m => m.OnEnchantmentCleared(ctx, player, targetCard, source));
    }

    public static Task OnCardTransformed(PlayerChoiceContext ctx, Player player, CardModel originalCard, CardModel? newCard, CardModel? source)
    {
        var cs = player.Creature.CombatState;
        if (cs == null) return Task.CompletedTask;
        return HookUtils.Dispatch<IOnCardTransformed>(cs, ctx,
            m => m.OnCardTransformed(ctx, player, originalCard, newCard, source));
    }
}
