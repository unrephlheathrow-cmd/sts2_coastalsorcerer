using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class BayTide : ShoreMagicianCardModel
{
    public BayTide() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithPower<NextTurnLessDrawPower>(3, 0, false);
        WithPower<DoubleDamagePower>(1, 0, true);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<NextTurnLessDrawPower>(choiceContext, Owner.Creature, DynamicVars["NextTurnLessDrawPower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<DoubleDamagePower>(choiceContext, Owner.Creature, DynamicVars["DoubleDamagePower"].IntValue, Owner.Creature, this);
    }
}
