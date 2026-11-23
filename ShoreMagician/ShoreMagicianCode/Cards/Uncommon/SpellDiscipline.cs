using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class SpellDiscipline : ShoreMagicianCardModel
{
    public SpellDiscipline() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        WithCostUpgradeBy(-1);
        WithKeywords(CardKeyword.Ethereal);
        WithVar("MagicGain", 2);
        WithPower<SpellDisciplinePower>(1, 0, false);
        WithTip(typeof(MagicPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<MagicPower>(choiceContext, Owner.Creature, DynamicVars["MagicGain"].IntValue, Owner.Creature, this);
        await CommonActions.ApplySelf<SpellDisciplinePower>(choiceContext, this);
    }
}
