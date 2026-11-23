using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class SpellDisciplinePower : ShoreMagicianPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyHandDraw(Player player, decimal count) =>
        player.Creature == Owner ? count + Amount : count;
}
