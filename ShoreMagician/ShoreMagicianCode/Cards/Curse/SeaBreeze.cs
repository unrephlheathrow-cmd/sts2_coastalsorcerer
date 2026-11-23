using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Cards.Curse;

[Pool(typeof(CurseCardPool))]
public sealed class SeaBreeze : ShoreMagicianCardModel
{
    public SeaBreeze() : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
    {
        WithKeywords(CardKeyword.Unplayable);
        WithVar("HpLoss", 1);
    }

    public override int MaxUpgradeLevel => 0;
    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(
            choiceContext,
            Owner.Creature,
            DynamicVars["HpLoss"].BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered,
            null,
            this,
            null);
    }
}
