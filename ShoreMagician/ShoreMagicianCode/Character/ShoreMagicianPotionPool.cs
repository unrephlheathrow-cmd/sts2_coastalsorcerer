using BaseLib.Abstracts;

namespace ShoreMagician.Code.Character;

public sealed class ShoreMagicianPotionPool : CustomPotionPoolModel
{
    public override string EnergyColorName => "shore_magician";
    public override string? BigEnergyIconPath => "res://ShoreMagician/images/character/energy_icon.png";
    public override string? TextEnergyIconPath => "res://ShoreMagician/images/character/text_energy_icon.png";
}
