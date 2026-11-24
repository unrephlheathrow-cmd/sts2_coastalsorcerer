using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class TwistFate : ShoreMagicianCardModel
{
    public TwistFate() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithPower<IntangiblePower>(1, 1, true);
    }

    protected override bool ShouldGlowGoldInternal =>
        Owner != null && IsInCombat && Owner.GetDiscardPile().Any(c => c.Type is CardType.Curse or CardType.Status);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var enchantedInDiscard = Owner.GetDiscardPile()
            .Where(c => c.Enchantment != null)
            .ToList();

        foreach (var card in enchantedInDiscard)
            await ShoreMagicianCmd.ClearEnchantment(choiceContext, Owner, card, this);

        var targetsInDiscard = Owner.GetDiscardPile()
            .Where(c => c.Type is CardType.Curse or CardType.Status)
            .ToList();

        var exhaustedCount = 0;
        foreach (var target in targetsInDiscard)
        {
            if (target.Pile?.Type != PileType.Discard)
                continue;

            await CardCmd.Exhaust(choiceContext, target);
            exhaustedCount++;
        }

        if (exhaustedCount > 0)
            await CommonActions.ApplySelf<IntangiblePower>(choiceContext, this);
    }
}
