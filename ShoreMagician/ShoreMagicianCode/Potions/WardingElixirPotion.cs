using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Potions;

/// <summary>
/// Rare potion: Transform all Curse and Status cards in your hand and draw pile
/// into 0-cost playable Sea Breeze cards with Exhaust.
/// </summary>
[Pool(typeof(ShoreMagicianPotionPool))]
public sealed class WardingElixirPotion : ShoreMagicianPotionModel
{
    private static readonly Color IndigoSplash = new(0.24f, 0.34f, 0.85f);

    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<SeaBreeze>(),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        NCombatRoom.Instance?.PlaySplashVfx(Owner.Creature, IndigoSplash);

        var targets = new List<CardModel>();
        targets.AddRange(PileType.Hand.GetPile(Owner).Cards.Where(IsCurseOrStatus));
        targets.AddRange(PileType.Draw.GetPile(Owner).Cards.Where(IsCurseOrStatus));

        foreach (var card in targets)
        {
            var converted = await ShoreMagicianCmd.TransformCardTo<SeaBreeze>(choiceContext, Owner, card, null);
            if (converted == null && card is SeaBreeze existingSeaBreeze)
                converted = existingSeaBreeze;

            if (converted == null)
                continue;

            converted.EnergyCost.SetCustomBaseCost(0);
            CardCmd.RemoveKeyword(converted, CardKeyword.Unplayable);
            CardCmd.ApplyKeyword(converted, CardKeyword.Exhaust);
        }
    }

    private static bool IsCurseOrStatus(CardModel card) =>
        card.Type is CardType.Curse or CardType.Status;
}
