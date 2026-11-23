using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Core;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Starter relic upgrade (via Touch of Orobas) replacing Spell Booklet (EnchantedBuckler).
/// Whenever you play an enchanted card, gain 3 Block.
/// </summary>
[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class ArcaneLexicon : ShoreMagicianRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner || !ShoreMagicianModel.WasEnchantedAtPlayStart(cardPlay.Card))
            return;

        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, 3m, ValueProp.Unpowered, null);
    }
}
