using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class DriftTurn : ShoreMagicianCardModel
{
    public DriftTurn() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("MomentumAmount", 8, 2);
        WithTips(e => HoverTipFactory.FromEnchantment<Momentum>(e.DynamicVars["MomentumAmount"].IntValue));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && c.Type == CardType.Attack && ShoreMagicianCmd.CanReplaceEnchantmentWith<Momentum>(c));

        if (chosen == null)
            return;

        await ShoreMagicianCmd.ReplaceEnchantment<Momentum>(
            choiceContext,
            Owner,
            chosen,
            DynamicVars["MomentumAmount"].IntValue,
            this);
        chosen.EnergyCost.AddThisCombat(-1);
    }
}
