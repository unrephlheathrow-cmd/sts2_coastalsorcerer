using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class PhantomShock : ShoreMagicianCardModel
{
    public PhantomShock() : base(2, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
    {
        WithBlock(5, 2);
        WithPower<WeakPower>(1, 0, true);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
        await CommonActions.CardBlock(this, cardPlay);

        var weakAmt = DynamicVars["WeakPower"].IntValue;
        await PowerCmd.Apply<WeakPower>(choiceContext, Owner.GetAllOpponents(), weakAmt, Owner.Creature, this);
    }
}
