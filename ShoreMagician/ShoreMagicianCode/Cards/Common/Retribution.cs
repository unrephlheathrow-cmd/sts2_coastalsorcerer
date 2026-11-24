using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Cards.Curse;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Commands;

namespace ShoreMagician.Code.Cards.Common;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class Retribution : ShoreMagicianCardModel
{
    public Retribution() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(15, 3);
        WithTips(_ => [HoverTipFactory.FromCard<SeaBreeze>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        var chosen = await CommonActions.SelectSingleCard(
            this,
            SelectionScreenPrompt,
            choiceContext,
            PileType.Draw,
            c => c.IsTransformable);

        if (chosen != null)
            await ShoreMagicianCmd.TransformCardTo<SeaBreeze>(choiceContext, Owner, chosen, this);
    }
}
