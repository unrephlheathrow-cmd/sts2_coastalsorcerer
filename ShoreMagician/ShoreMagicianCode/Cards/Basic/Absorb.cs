using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Ancient;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Basic;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Absorb : ShoreMagicianCardModel, ITranscendenceCard
{
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<Levee>();
    public Absorb() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithVar("AdroitAmount", 3, 1);
        WithTips(e => HoverTipFactory.FromEnchantment<Adroit>(e.DynamicVars["AdroitAmount"].IntValue));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Hand,
            c => c != this && ShoreMagicianCmd.CanEnchantWith<Adroit>(c));

        if (chosen != null)
            await ShoreMagicianCmd.EnchantCard<Adroit>(choiceContext, Owner, chosen, DynamicVars["AdroitAmount"].IntValue, this);
    }
}
