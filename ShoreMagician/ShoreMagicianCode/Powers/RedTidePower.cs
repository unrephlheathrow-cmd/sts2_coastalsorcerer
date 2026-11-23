using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Core;
using ShoreMagician.Code.Events;

namespace ShoreMagician.Code.Powers;

public sealed class RedTidePower : ShoreMagicianPowerModel, IOnCardEnchanted, IOnEnchantmentCleared, IOnCardTransformed
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    private async Task TryGrantBlockAsync(Player player)
    {
        if (player.Creature != Owner)
            return;

        if (!ShoreMagicianModel.TryConsumeRedTideTrigger(player, 4))
            return;

        Flash();
        await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }

    public Task OnCardEnchanted(PlayerChoiceContext ctx, Player player, CardModel targetCard, EnchantmentModel enchantment, int amount, CardModel? source) =>
        TryGrantBlockAsync(player);

    public Task OnEnchantmentCleared(PlayerChoiceContext ctx, Player player, CardModel targetCard, CardModel? source) =>
        TryGrantBlockAsync(player);

    public Task OnCardTransformed(PlayerChoiceContext ctx, Player player, CardModel originalCard, CardModel? newCard, CardModel? source) =>
        TryGrantBlockAsync(player);
}
