using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Browse : ShoreMagicianCardModel
{
    public Browse() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(3, 1);
        WithBlock(2, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        var toDiscard = Owner.GetHand().Where(c => c != this && c.Enchantment == null).ToList();
        if (toDiscard.Count == 0)
            return;

        await CardCmd.Discard(choiceContext, toDiscard);
        for (var i = 0; i < toDiscard.Count; i++)
            await CommonActions.CardBlock(this, cardPlay);
    }
}
