using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class MorphStrike : ShoreMagicianCardModel
{
    public MorphStrike() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(15, 3);
        WithCards(2, 0);
        WithVar("TransformCount", 2);
        WithTags(CardTag.Strike);
        WithEnchantmentTip<Steady>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

        var maxTransform = DynamicVars["TransformCount"].IntValue;
        var toTransform = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.IsTransformable,
            0,
            maxTransform);

        foreach (var card in toTransform.ToList())
            await ShoreMagicianCmd.TransformCard(choiceContext, Owner, card, this);

        var enchantPrompt = new LocString("cards", $"{Id.Entry}.enchantPrompt");
        var enchantTarget = await CommonActions.SelectSingleCard(
            this,
            enchantPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && ShoreMagicianCmd.CanReplaceEnchantmentWith<Steady>(c));

        if (enchantTarget != null)
            await ShoreMagicianCmd.ReplaceEnchantment<Steady>(choiceContext, Owner, enchantTarget, 1, this);
    }
}
