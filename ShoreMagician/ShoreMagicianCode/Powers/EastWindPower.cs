using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Powers;

public sealed class EastWindPower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];

    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (shuffler.Creature != Owner || Owner.Player == null || Amount <= 0)
            return;

        var enemies = Owner.Player.GetAllOpponents();
        if (enemies.Count == 0)
            return;

        Flash();
        await PowerCmd.Apply<WeakPower>(choiceContext, enemies, Amount, Owner, null);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, enemies, Amount, Owner, null);
    }
}
