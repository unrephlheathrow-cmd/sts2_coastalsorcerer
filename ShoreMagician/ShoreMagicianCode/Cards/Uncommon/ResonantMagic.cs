using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Powers;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ResonantMagic : ShoreMagicianCardModel
{
    public ResonantMagic() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ResonantMagicPower>(1, 1, false);
        WithTips(_ => [HoverTipFactory.FromPower<TempMagicPower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<ResonantMagicPower>(choiceContext, this);
        var power = Owner.Creature.GetPower<ResonantMagicPower>();
        if (power != null)
            power.IgnoreCurrentPlayCard = this;
    }
}
