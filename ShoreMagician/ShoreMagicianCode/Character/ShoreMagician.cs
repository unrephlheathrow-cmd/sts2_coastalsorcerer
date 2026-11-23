using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using ShoreMagician.Code.Cards.Basic;
using ShoreMagician.Code.Relics;

namespace ShoreMagician.Code.Character;

public sealed class ShoreMagician : PlaceholderCharacterModel
{
    public static readonly Color PrussianBlue = new(0f, 0.192f, 0.325f);

    public override Color NameColor => new(0.28f, 0.56f, 0.85f);
    public override Color EnergyLabelOutlineColor => PrussianBlue;
    public override Color MapDrawingColor => new(0.10f, 0.36f, 0.60f);

    public override CharacterGender Gender => CharacterGender.Neutral;
    protected override CharacterModel? UnlocksAfterRunAs => null;

    public override int StartingHp => 75;
    public override int StartingGold => 99;
    public override float AttackAnimDelay => 0.15f;
    public override float CastAnimDelay => 0.25f;

    public override string CustomVisualPath => "res://ShoreMagician/scenes/shore_magician.tscn";
    public override string CustomCharacterSelectBg => "res://ShoreMagician/scenes/char_select_bg.tscn";
    public override string CustomCharacterSelectIconPath => "res://ShoreMagician/images/character/char_select_icon.png";
    public override string CustomCharacterSelectLockedIconPath => "res://ShoreMagician/images/character/char_select_locked.png";
    public override string CustomIconPath => "res://ShoreMagician/scenes/char_icon.tscn";
    public override string CustomIconTexturePath => "res://ShoreMagician/images/character/char_icon.png";
    public override string CustomIconOutlineTexturePath => "res://ShoreMagician/images/character/char_icon_outline.png";
    public override string CustomEnergyCounterPath => "res://ShoreMagician/scenes/energy_counter.tscn";
    public override string CustomMapMarkerPath => "res://ShoreMagician/images/character/map_marker.png";
    public override string CustomRestSiteAnimPath => "res://ShoreMagician/scenes/rest_site.tscn";
    public override string CustomMerchantAnimPath => "res://ShoreMagician/scenes/merchant.tscn";
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    public override CardPoolModel CardPool => ModelDb.CardPool<ShoreMagicianCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<ShoreMagicianRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<ShoreMagicianPotionPool>();

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeShoreMagician>(),
        ModelDb.Card<StrikeShoreMagician>(),
        ModelDb.Card<StrikeShoreMagician>(),
        ModelDb.Card<StrikeShoreMagician>(),
        ModelDb.Card<StrikeShoreMagician>(),
        ModelDb.Card<DefendShoreMagician>(),
        ModelDb.Card<DefendShoreMagician>(),
        ModelDb.Card<DefendShoreMagician>(),
        ModelDb.Card<DefendShoreMagician>(),
        ModelDb.Card<DefendShoreMagician>(),
        ModelDb.Card<Affix>(),
        ModelDb.Card<Absorb>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<EnchantedBuckler>()
    ];

    public override List<string> GetArchitectAttackVfx() =>
    [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];
}
