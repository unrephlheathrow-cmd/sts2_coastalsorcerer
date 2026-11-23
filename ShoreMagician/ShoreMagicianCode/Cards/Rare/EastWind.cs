using BaseLib.Abstracts;
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
public sealed class EastWind : ShoreMagicianCardModel
{
    public EastWind() : base(0, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithCards(1, 0);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithPower<EastWindPower>(1, 0, false);
        WithTips(_ => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        await CommonActions.ApplySelf<EastWindPower>(choiceContext, this);
    }
}
