using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ThornTrap : ShoreMagicianCardModel
{
    public ThornTrap() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(18, 3);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        if (CombatState == null || CombatManager.Instance.IsOverOrEnding || Owner.GetAllOpponents().Count == 0)
            return;

        var exhaustPile = PileType.Exhaust.GetPile(Owner);
        if (!exhaustPile.Cards.Any(c => c.Enchantment != null))
            return;

        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1) { Cancelable = true };
        var selected = (await CardSelectCmd.FromCombatPile(
            choiceContext,
            exhaustPile,
            Owner,
            prefs,
            c => c.Enchantment != null)).FirstOrDefault();

        if (selected == null)
            return;

        var target = selected.TargetType == TargetType.AnyEnemy
            ? (cardPlay.Target?.IsAlive == true ? cardPlay.Target : Owner.RunState.Rng.CombatTargets.NextItem(Owner.GetAllOpponents()))
            : null;
        await CardCmd.AutoPlay(choiceContext, selected, target);
    }
}
