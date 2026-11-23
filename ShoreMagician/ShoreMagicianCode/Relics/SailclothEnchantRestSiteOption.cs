using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;

namespace ShoreMagician.Code.Relics;

public sealed class SailclothEnchantRestSiteOption(Player owner) : RestSiteOption(owner)
{
    public const string CustomIconPath = "res://ShoreMagician/images/ui/option_sailcloth_enchant.png";

    public override string OptionId => "SAILCLOTH_ENCHANT";

    public override IEnumerable<string> AssetPaths => [CustomIconPath];

    public override bool IsEnabled =>
        PileType.Deck.GetPile(Owner).Cards.Any(c => ModelDb.Enchantment<Swift>().CanEnchant(c));

    public override async Task<bool> OnSelect()
    {
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1)
        {
            Cancelable = true,
            RequireManualConfirmation = true
        };

        var swift = ModelDb.Enchantment<Swift>();
        var selected = (await CardSelectCmd.FromDeckForEnchantment(Owner, swift, Sailcloth.SwiftAmount, prefs)).ToList();
        if (selected.Count == 0)
            return false;

        foreach (var card in selected)
        {
            if (!swift.CanEnchant(card))
                continue;

            CardCmd.Enchant<Swift>(card, Sailcloth.SwiftAmount);
            if (!TestMode.IsOn)
            {
                SfxCmd.Play("event:/sfx/ui/cards/card_transform");
                var vfx = NCardEnchantVfx.Create(card);
                if (vfx != null)
                    NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(vfx);
            }
        }

        Owner.GetRelic<Sailcloth>()?.Flash();
        return true;
    }
}
