using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace ShoreMagician.Code.Rewards;

public sealed class EnchantPerfectFitReward : DeckEnchantmentRewardBase<PerfectFit>
{
    [CustomEnum]
    public static RewardType PerfectFitEnchant;

    public EnchantPerfectFitReward() : base(null!, 1)
    {
    }

    public EnchantPerfectFitReward(Player player) : base(player, 1)
    {
    }

    protected override RewardType RewardType => PerfectFitEnchant;
    protected override string DescriptionKey => "SHOREMAGICIAN-COMBAT_REWARD_ENCHANT_PERFECT_FIT";
    protected override string SelectionPromptKey => "SHOREMAGICIAN-FOUNTAIN_REPAY.selectionScreenPrompt";

    public override CreateRewardFromSave<CustomReward> DeserializeMethod => CreateFromSerializable;

    public static CustomReward CreateFromSerializable(SerializableReward save, Player player) =>
        new EnchantPerfectFitReward(player);
}
