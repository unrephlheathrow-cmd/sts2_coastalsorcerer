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
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Potions;

/// <summary>
/// Common potion: Enchant an Attack card in hand with Sharp 15.
/// Can be drunk even if there are no eligible Attack cards in hand.
/// </summary>
[Pool(typeof(ShoreMagicianPotionPool))]
public sealed class ConcentratedMagicPotion : ShoreMagicianPotionModel
{
    private const int BaseSharpAmount = 15;
    private static readonly Color IndigoSplash = new(0.24f, 0.34f, 0.85f);

    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("SharpAmount", BaseSharpAmount)
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
        HoverTipFactory.FromEnchantment<Sharp>(DynamicVars["SharpAmount"].IntValue);

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, IndigoSplash);

        var hasEligible = PileType.Hand.GetPile(Owner).Cards.Any(ShoreMagicianCmd.CanEnchantWith<Sharp>);
        if (!hasEligible)
            return;

        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var chosen = (await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            prefs,
            ShoreMagicianCmd.CanEnchantWith<Sharp>,
            this)).FirstOrDefault();

        if (chosen == null)
            return;

        await ShoreMagicianCmd.EnchantCard<Sharp>(
            choiceContext,
            Owner,
            chosen,
            DynamicVars["SharpAmount"].IntValue,
            null);
    }
}
