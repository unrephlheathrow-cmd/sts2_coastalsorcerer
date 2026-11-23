using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace ShoreMagician.Code.Rewards;

public sealed class EnchantAdroitReward : DeckEnchantmentRewardBase<Adroit>
{
    [CustomEnum]
    public static RewardType AdroitEnchant;

    public EnchantAdroitReward() : base(null!, 2)
    {
    }

    public EnchantAdroitReward(Player player, int amount = 2) : base(player, amount)
    {
    }

    protected override RewardType RewardType => AdroitEnchant;
    protected override string DescriptionKey => "SHOREMAGICIAN-COMBAT_REWARD_ENCHANT_ADROIT";
    protected override string SelectionPromptKey => "SHOREMAGICIAN-EXPERIENCE_SUMMARY.selectionScreenPrompt";

    public override CreateRewardFromSave<CustomReward> DeserializeMethod => CreateFromSerializable;

    public static CustomReward CreateFromSerializable(SerializableReward save, Player player) =>
        new EnchantAdroitReward(player, save.GoldAmount > 0 ? save.GoldAmount : 2);
}
