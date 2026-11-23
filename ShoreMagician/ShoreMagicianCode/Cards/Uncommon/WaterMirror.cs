using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;

namespace ShoreMagician.Code.Cards.Uncommon;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class WaterMirror : ShoreMagicianCardModel
{
    public WaterMirror() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(9, 3);
        WithVar("CardCount", 3);
        WithTips(card => card.IsUpgraded
            ? [HoverTipFactory.FromKeyword(CardKeyword.Ethereal)]
            : []);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var count = DynamicVars["CardCount"].IntValue;
        var rawCandidates = ModelDb.AllCharacterCardPools
            .Where(p => p != Owner.Character.CardPool)
            .SelectMany(p => p.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint))
            .Where(c => c.Type == CardType.Skill
                && c.Rarity is CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare
                && c.CanBeGeneratedInCombat)
            .DistinctBy(c => c.Id);

        var pool = CardFactory.FilterForRun(Owner.RunState, rawCandidates).ToList();
        if (pool.Count == 0 || CombatState == null)
            return;

        var rng = Owner.RunState.Rng.CombatCardGeneration;
        var chosenTemplates = new List<CardModel>(count);
        var remaining = pool.ToList();
        for (var i = 0; i < count; i++)
        {
            var sourceList = remaining.Count > 0 ? remaining : pool;
            var picked = rng.NextItem(sourceList);
            if (picked == null)
                break;

            chosenTemplates.Add(picked);
            remaining.Remove(picked);
        }

        if (chosenTemplates.Count == 0)
            return;

        var created = chosenTemplates.Select(t => CombatState.CreateCard(t, Owner)).ToList();
        if (IsUpgraded)
        {
            foreach (var card in created)
                CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
        }

        var results = (await CardPileCmd.AddGeneratedCardsToCombat(created, PileType.Hand, Owner, CardPilePosition.Bottom)).ToList();
        if (results.Count > 0)
            CardCmd.PreviewCardPileAdd(results);
    }
}
