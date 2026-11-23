using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class ConvergeImpact : ShoreMagicianCardModel
{
    private int _pendingSharpAmount;

    public ConvergeImpact() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6, 0);
        WithVar("SharpBase", 6, 2);
        WithTips(e => HoverTipFactory.FromEnchantment<Sharp>(e.DynamicVars["SharpBase"].IntValue));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 1. Deal damage first (includes any pre-existing enchantment on this card).
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitCount(2)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        // 2. Clear enchantment from up to 3 cards in hand.
        var chosen = await CommonActions.SelectCards(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.Enchantment != null,
            0,
            3);

        var clearedCount = 0;
        foreach (var card in chosen.ToList())
        {
            if (await ShoreMagicianCmd.ClearEnchantment(choiceContext, Owner, card, this))
                clearedCount++;
        }

        // 3. Defer applying Sharp until AfterCardPlayed (after Powers/Relics have finished observing this play).
        _pendingSharpAmount = DynamicVars["SharpBase"].IntValue + Math.Min(3, clearedCount);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != this || _pendingSharpAmount <= 0 || Owner == null)
            return;

        var sharpAmount = _pendingSharpAmount;
        _pendingSharpAmount = 0;
        await ShoreMagicianCmd.ReplaceEnchantment<Sharp>(choiceContext, Owner, this, sharpAmount, this);
    }
}
