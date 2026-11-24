using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class DazzlingFlash : ShoreMagicianCardModel
{
    public DazzlingFlash() : base(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
    {
        WithPower<WeakPower>(1, 1, true);
        WithPower<TempMagicPower>(2, 0, true);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WeakPower>(choiceContext, Owner.GetAllOpponents(), DynamicVars["WeakPower"].IntValue, Owner.Creature, this);
        await CommonActions.ApplySelf<TempMagicPower>(choiceContext, this);
    }
}
