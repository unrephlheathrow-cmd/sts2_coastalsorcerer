using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Potions;

/// <summary>
/// Uncommon potion: Transform up to 3 cards in your hand into random 0-cost cards.
/// </summary>
[Pool(typeof(ShoreMagicianPotionPool))]
public sealed class DistortedMemoryPotion : ShoreMagicianPotionModel
{
    private static readonly Color IndigoSplash = new(0.24f, 0.34f, 0.85f);

    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Transform)
    ];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, IndigoSplash);

        var maxCount = DynamicVars.Cards.IntValue;
        var hasEligible = PileType.Hand.GetPile(Owner).Cards.Any(c => c.IsTransformable);
        if (maxCount <= 0 || !hasEligible)
            return;

        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, maxCount);
        var chosen = (await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            prefs,
            c => c.IsTransformable,
            this)).ToList();

        foreach (var card in chosen)
            await ShoreMagicianCmd.TransformToZeroCostCard(choiceContext, Owner, card, null);
    }
}
