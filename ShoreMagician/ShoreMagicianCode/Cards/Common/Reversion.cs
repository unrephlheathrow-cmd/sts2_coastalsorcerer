using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Reversion : ShoreMagicianCardModel
{
    public Reversion() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithEnergy(3);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        WithEnergyTip();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.Enchantment != null);

        if (chosen != null && await ShoreMagicianCmd.ClearEnchantment(choiceContext, Owner, chosen, this))
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }

    public override CardLocation ModifyCardPlayResultLocation(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        CardLocation cardLocation)
    {
        if (card == this && IsUpgraded)
            return new CardLocation(cardLocation.player, PileType.None, CardPilePosition.None);

        return cardLocation;
    }
}
