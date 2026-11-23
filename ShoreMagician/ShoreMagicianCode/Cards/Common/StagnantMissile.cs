using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class StagnantMissile : ShoreMagicianCardModel
{
    public StagnantMissile() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(15, 2);
        WithTips(_ => [HoverTipFactory.FromPower<SlowPower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != null)
            await PowerCmd.Apply<SlowPower>(choiceContext, cardPlay.Target, 1m, Owner.Creature, this);

        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
    }
}
