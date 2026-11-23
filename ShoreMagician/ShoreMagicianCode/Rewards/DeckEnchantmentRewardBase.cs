using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Rewards;

public abstract class DeckEnchantmentRewardBase<TEnchantment> : CustomReward where TEnchantment : EnchantmentModel
{
    protected DeckEnchantmentRewardBase(Player player, int amount = 1) : base(player)
    {
        Amount = amount;
    }

    public int Amount { get; set; }

    protected abstract string DescriptionKey { get; }
    protected abstract string SelectionPromptKey { get; }
    protected virtual string SelectionPromptTable => "cards";

    public override bool IsPopulated => true;

    protected override string IconPath => "res://ShoreMagician/images/rewards/enchant_adroit_reward.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<TEnchantment>(Amount);

    public override LocString Description
    {
        get
        {
            var loc = new LocString("gameplay_ui", DescriptionKey);
            loc.Add("Amount", (decimal)Amount);
            return loc;
        }
    }

    public override SerializableReward ToSerializable() => new()
    {
        RewardType = RewardType,
        GoldAmount = Amount
    };

    public override void Populate()
    {
    }

    public override void MarkContentAsSeen()
    {
    }

    protected override async Task<bool> OnSelect()
    {
        var candidates = Player.Deck.Cards
            .Where(ShoreMagicianCmd.CanEnchantWith<TEnchantment>)
            .ToList();

        if (candidates.Count == 0)
            return true;

        var prompt = new LocString(SelectionPromptTable, SelectionPromptKey);
        var prefs = new CardSelectorPrefs(prompt, 1)
        {
            Cancelable = true,
            RequireManualConfirmation = true
        };

        var chosen = (await CardSelectCmd.FromDeckForEnchantment(
            Player,
            ModelDb.Enchantment<TEnchantment>(),
            Amount,
            c => c != null && ShoreMagicianCmd.CanEnchantWith<TEnchantment>(c),
            prefs)).ToList();

        if (chosen.Count == 0)
            return false;

        foreach (var card in chosen)
        {
            CardCmd.Enchant<TEnchantment>(card, Amount);
            CardCmd.Preview(card);
        }

        return true;
    }
}
