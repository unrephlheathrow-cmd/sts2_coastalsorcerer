using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class FountainRepay : ShoreMagicianCardModel
{
    public override bool CanBeGeneratedInCombat => false;

    public FountainRepay() : base(0, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
        WithPower<FountainRepayPower>(2, 0, false);
        WithTips(_ => HoverTipFactory.FromEnchantment<PerfectFit>(1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var existing = Owner.Creature.GetPower<FountainRepayPower>();
        if (existing == null)
        {
            var applied = await PowerCmd.Apply<FountainRepayPower>(choiceContext, Owner.Creature, 2m, Owner.Creature, this);
            applied?.RegisterCast();
            return;
        }

        existing.RegisterCast();
        var delta = existing.DesiredDisplayAmount - existing.Amount;
        if (delta != 0)
            await PowerCmd.ModifyAmount(choiceContext, existing, delta, Owner.Creature, this, true);
    }
}
