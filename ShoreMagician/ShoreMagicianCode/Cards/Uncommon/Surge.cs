using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Surge : ShoreMagicianCardModel
{
    public Surge() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(8, 0);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        WithEnchantmentTip<Sharp>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitCount(2)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var rng = Owner.RunState.Rng.CombatCardGeneration;
        foreach (var card in Owner.GetHand().Where(c => c != this && c.Type == CardType.Attack && ShoreMagicianCmd.CanEnchantWith<Sharp>(c)).ToList())
        {
            var amount = rng.NextInt(1, 6);
            await ShoreMagicianCmd.EnchantCard<Sharp>(choiceContext, Owner, card, amount, this);
        }
    }
}
