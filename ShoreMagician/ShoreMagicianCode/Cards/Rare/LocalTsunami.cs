using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using ShoreMagician.Code.Abstract;
using ShoreMagician.Code.Character;
using ShoreMagician.Code.Extensions;

namespace ShoreMagician.Code.Cards.Rare;

[Pool(typeof(ShoreMagicianCardPool))]
public sealed class LocalTsunami : ShoreMagicianCardModel
{
    public LocalTsunami() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(15, 0);
        WithKeywords(CardKeyword.Exhaust);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithVar("HpThreshold", 20);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);

        var threshold = DynamicVars["HpThreshold"].IntValue;
        foreach (var enemy in Owner.GetAllOpponents())
        {
            if (enemy.IsAlive
                && enemy.CurrentHp < threshold
                && enemy.Monster?.MoveStateMachine?.StateLog.Count > 0)
            {
                await CreatureCmd.Stun(enemy);
            }
        }
    }
}
