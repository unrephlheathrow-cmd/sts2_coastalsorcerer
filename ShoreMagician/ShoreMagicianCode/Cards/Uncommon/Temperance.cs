using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Temperance : ShoreMagicianCardModel
{
    public Temperance() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(3, 0);
        WithVar("NextTurnDraw", 2);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["NextTurnDraw"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<NoDrawPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }
}
