using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class SpellObsession : ShoreMagicianCardModel
{
    public SpellObsession() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Ethereal);
        WithVar("StatLoss", 2, -1);
        WithPower<MagicPower>(3, 0, true);
        WithTips(_ => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var loss = DynamicVars["StatLoss"].IntValue;
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, -loss, Owner.Creature, this);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, -loss, Owner.Creature, this);
        await CommonActions.ApplySelf<MagicPower>(choiceContext, this);
    }
}
