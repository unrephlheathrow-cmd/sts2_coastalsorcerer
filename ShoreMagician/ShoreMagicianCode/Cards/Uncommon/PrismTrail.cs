using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class PrismTrail : ShoreMagicianCardModel
{
    public PrismTrail() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(10, 2);
        WithCards(2, 1);
        WithVar("TransformCount", 2);
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        var transformCount = DynamicVars["TransformCount"].IntValue;
        var eligibleCount = Owner.GetHand().Count(c => c != this && c.IsTransformable);
        if (eligibleCount == 0)
            return;

        var targetCount = Math.Min(transformCount, eligibleCount);
        var chosen = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.IsTransformable,
            targetCount,
            targetCount);

        foreach (var card in chosen.ToList())
            await ShoreMagicianCmd.TransformToRareSameType(choiceContext, Owner, card, this);
    }
}
