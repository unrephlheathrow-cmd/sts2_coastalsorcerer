using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class UndercurrentStrike : ShoreMagicianCardModel
{
    private bool _playedThisCombat;

    public UndercurrentStrike() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(25, 5);
        WithTags(CardTag.Strike);
        WithEnchantmentTip<Sharp>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var isFirstPlay = !_playedThisCombat;
        _playedThisCombat = true;

        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await ShoreMagicianCmd.DoubleSharpEnchantment(choiceContext, Owner, this, this);

        if (IsUpgraded && isFirstPlay)
            EnergyCost.AddThisCombat(-1);
    }
}
