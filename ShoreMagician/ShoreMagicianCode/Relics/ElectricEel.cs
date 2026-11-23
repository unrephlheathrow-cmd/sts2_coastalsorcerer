using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Relics;

/// <summary>
/// Rare relic: The first time you draw a Curse each combat, draw 2 cards.
/// </summary>
[Pool(typeof(ShoreMagicianRelicPool))]
public sealed class ElectricEel : ShoreMagicianRelicModel
{
    private bool _triggeredThisCombat;

    public override RelicRarity Rarity => RelicRarity.Rare;

    public override Task BeforeCombatStart()
    {
        _triggeredThisCombat = false;
        Status = RelicStatus.Active;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        _triggeredThisCombat = false;
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (_triggeredThisCombat || card.Owner != Owner || card.Type != CardType.Curse)
            return;

        _triggeredThisCombat = true;
        Status = RelicStatus.Normal;
        Flash();
        await CardPileCmd.Draw(choiceContext, 2m, Owner);
    }
}
