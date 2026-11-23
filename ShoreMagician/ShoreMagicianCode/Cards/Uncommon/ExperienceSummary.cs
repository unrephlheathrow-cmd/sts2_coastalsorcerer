using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ExperienceSummary : ShoreMagicianCardModel
{
    public override bool CanBeGeneratedInCombat => false;

    public ExperienceSummary() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        WithPower<ExperienceSummaryPower>(1, 0, false);
        WithVar("AdroitAmount", 2, 1);
        WithTips(e => HoverTipFactory.FromEnchantment<Adroit>(e.DynamicVars["AdroitAmount"].IntValue));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
            await PowerCmd.Apply<ExperienceSummaryUpgradedPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        else
            await CommonActions.ApplySelf<ExperienceSummaryPower>(choiceContext, this);
    }
}
