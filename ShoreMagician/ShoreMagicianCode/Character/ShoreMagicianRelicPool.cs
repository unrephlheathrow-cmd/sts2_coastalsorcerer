using BaseLib.Abstracts;
using Godot;

namespace ShoreMagician.Code.Character;

public sealed class ShoreMagicianRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => "shore_magician";
    public override Color LabOutlineColor => new(0f, 0.192f, 0.325f);
    public override string? BigEnergyIconPath => "res://ShoreMagician/images/character/energy_icon.png";
    public override string? TextEnergyIconPath => "res://ShoreMagician/images/character/text_energy_icon.png";
}
