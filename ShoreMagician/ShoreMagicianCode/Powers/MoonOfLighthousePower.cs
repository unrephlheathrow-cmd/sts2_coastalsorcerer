using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Random;
using ShoreMagician.Code.Abstract;

namespace ShoreMagician.Code.Powers;

public sealed class MoonOfLighthousePower : ShoreMagicianPowerModel
{
    private static readonly Func<CardModel>[] CurseTransformPool =
    [
        ModelDb.Card<Fuel>,
        ModelDb.Card<MinionSacrifice>,
        ModelDb.Card<Joy>,
        ModelDb.Card<Soul>,
        ModelDb.Card<Dazed>,
    ];

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<Fuel>(),
        HoverTipFactory.FromCard<MinionSacrifice>(),
        HoverTipFactory.FromCard<Joy>(),
        HoverTipFactory.FromCard<Soul>(),
        HoverTipFactory.FromCard<Dazed>(),
    ];

    public static CardModel? TryCreateCurseReplacement(CardModel original, Rng rng)
    {
        var owner = original.Owner;
        var cardScope = original.CardScope;
        if (original.Type != CardType.Curse || owner == null || cardScope == null)
            return null;

        var power = owner.Creature.GetPower<MoonOfLighthousePower>();
        if (power == null)
            return null;

        var factory = rng.NextItem(CurseTransformPool);
        if (factory == null)
            return null;

        power.Flash();
        return cardScope.CreateCard(factory(), owner);
    }
}
