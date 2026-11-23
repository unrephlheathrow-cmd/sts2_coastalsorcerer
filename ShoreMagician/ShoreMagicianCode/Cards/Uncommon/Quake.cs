using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Quake : ShoreMagicianCardModel
{
    public Quake() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(21, 5);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var hadBlock = Owner.GetAllOpponents().Where(e => e.Block > 0).ToHashSet();

        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_heavy_blunt")
            .Execute(choiceContext);

        foreach (var enemy in hadBlock)
        {
            if (enemy.IsAlive && enemy.Block == 0 && enemy.Monster?.MoveStateMachine?.StateLog.Count > 0)
                await CreatureCmd.Stun(enemy);
        }
    }
}
