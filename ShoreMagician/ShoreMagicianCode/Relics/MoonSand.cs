using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Uncommon relic: Sea Breeze can be played for 0 Energy and Exhausts.
/// Energy cost override from -1 to 0 is handled by RelicMechanicsPatch on CardEnergyCost.GetWithModifiers.
/// </summary>
[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class MoonSand : ShoreMagicianRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<SeaBreeze>(),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
    ];

    public override bool TryModifyKeywordsInCombat(CardModel card, ISet<CardKeyword> keywords)
    {
        if (card.Owner != Owner || card is not SeaBreeze)
            return false;

        keywords.Remove(CardKeyword.Unplayable);
        keywords.Add(CardKeyword.Exhaust);
        return true;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && cardPlay.Card is SeaBreeze)
            Flash();

        return Task.CompletedTask;
    }
}
