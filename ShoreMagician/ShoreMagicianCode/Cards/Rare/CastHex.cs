using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class CastHex : ShoreMagicianCardModel
{
    public CastHex() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(18, 0);
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
        WithPower<WeakPower>(3, 0, true);
        WithPower<VulnerablePower>(3, 0, true);
        WithPower<SlowPower>(1, 0, true);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);

        if (cardPlay.Target == null)
            return;

        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars["WeakPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars["VulnerablePower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<SlowPower>(choiceContext, cardPlay.Target, DynamicVars["SlowPower"].IntValue, Owner.Creature, this);
    }
}
