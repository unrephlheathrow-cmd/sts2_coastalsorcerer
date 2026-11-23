using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Core;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Overflow : ShoreMagicianCardModel
{
    public Overflow() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithDamage(4, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ShoreMagicianModel.MarkOverflowPlayed(Owner);
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitCount(3)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }
}
