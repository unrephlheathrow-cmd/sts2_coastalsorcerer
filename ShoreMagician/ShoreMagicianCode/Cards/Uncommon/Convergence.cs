using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Convergence : ShoreMagicianCardModel
{
    public Convergence() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithEnergy(2);
        WithCards(2, 1);
        WithEnergyTip();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Draw,
            _ => true);

        if (chosen == null)
            return;

        var power = await PowerCmd.Apply<ConvergencePower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
        power?.RegisterTarget(chosen, DynamicVars.Energy.IntValue, DynamicVars.Cards.IntValue);
    }
}
