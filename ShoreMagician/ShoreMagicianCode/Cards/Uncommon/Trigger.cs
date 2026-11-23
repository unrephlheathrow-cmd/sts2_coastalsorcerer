using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Trigger : ShoreMagicianCardModel
{
    public Trigger() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("AdroitAmount", 3);
        WithEnchantmentTip<Adroit>(3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var candidates = Owner.GetHand()
            .Where(c => c != this
                && c.Type == CardType.Skill
                && c.Enchantment == null
                && !c.Keywords.Contains(CardKeyword.Unplayable)
                && ShoreMagicianCmd.CanEnchantWith<Adroit>(c))
            .ToList();

        if (candidates.Count == 0)
            return;

        if (IsUpgraded)
        {
            var currentEnergy = Math.Max(0, Owner.PlayerCombatState?.Energy ?? 0);
            int EffectiveRankCost(CardModel c) => c.EnergyCost.CostsX
                ? currentEnergy
                : Math.Max(0, c.EnergyCost.GetWithModifiers(CostModifiers.All));

            var maxCost = candidates.Max(EffectiveRankCost);
            candidates = candidates.Where(c => EffectiveRankCost(c) == maxCost).ToList();
        }

        var chosen = Owner.RunState.Rng.CombatCardGeneration.NextItem(candidates);
        if (chosen == null)
            return;

        await ShoreMagicianCmd.EnchantCard<Adroit>(choiceContext, Owner, chosen, DynamicVars["AdroitAmount"].IntValue, this);

        var target = chosen.TargetType == TargetType.AnyEnemy
            ? Owner.RunState.Rng.CombatTargets.NextItem(Owner.GetAllOpponents())
            : null;
        await CardCmd.AutoPlay(choiceContext, chosen, target);
    }
}
