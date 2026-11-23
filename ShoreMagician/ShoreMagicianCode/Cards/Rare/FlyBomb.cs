using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class FlyBomb : ShoreMagicianCardModel
{
    public FlyBomb() : base(2, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithDamage(21, 4);
        WithVars(
            new CalculationBaseVar(0m),
            new CalculationExtraVar(1m),
            new CalculatedHitCountVar("CalculatedHits")
                .WithMultiplier((card, _) => card.Owner?.GetDiscardPile().Count(c => c.Enchantment != null) ?? 0));
        WithTips(_ => [HoverTipFactory.FromCard<SeaBreeze>(), HoverTipFactory.FromPower<StrengthPower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var discardCards = Owner.GetDiscardPile().ToList();
        var enchantedHits = 0;
        var seaBreezeCount = 0;

        foreach (var card in discardCards)
        {
            if (card.Pile?.Type != PileType.Discard)
                continue;

            var hadEnchantment = card.Enchantment != null;
            var isSeaBreeze = card is SeaBreeze;
            await CardCmd.Exhaust(choiceContext, card);
            if (hadEnchantment)
                enchantedHits++;
            if (isSeaBreeze)
                seaBreezeCount++;
        }

        if (enchantedHits > 0 && CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(enchantedHits)
                .FromCard(this, cardPlay)
                .TargetingRandomOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }

        if (seaBreezeCount > 0)
        {
            var opponents = Owner.GetAllOpponents();
            if (opponents.Count > 0)
                await PowerCmd.Apply<StrengthPower>(choiceContext, opponents, -seaBreezeCount, Owner.Creature, this);
        }
    }
}
