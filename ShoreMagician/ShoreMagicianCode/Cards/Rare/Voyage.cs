using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Voyage : ShoreMagicianCardModel
{
    public Voyage() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithPower<DexterityPower>(1, 0, true);
        WithPower<RetainHandPower>(2, 1, true);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var dexLoss = DynamicVars["DexterityPower"].IntValue;
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, -dexLoss, Owner.Creature, this);
        await PowerCmd.Apply<RetainHandPower>(choiceContext, Owner.Creature, DynamicVars["RetainHandPower"].IntValue, Owner.Creature, this);
    }
}
