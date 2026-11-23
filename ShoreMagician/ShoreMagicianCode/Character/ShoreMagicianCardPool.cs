using BaseLib.Abstracts;
using Godot;

namespace ShoreMagician.Code.Character;

public sealed class ShoreMagicianCardPool : CustomCardPoolModel
{
    public override string Title => "shore_magician";

    public override float H => 0.567f;
    public override float S => 0.78f;
    public override float V => 0.62f;

    public override Color DeckEntryCardColor => new(0f, 0.192f, 0.325f);
    public override Color ShaderColor => new(0.06f, 0.28f, 0.50f);

    public override bool IsColorless => false;

    public override string BigEnergyIconPath => "res://ShoreMagician/images/character/energy_icon.png";
    public override string TextEnergyIconPath => "res://ShoreMagician/images/character/text_energy_icon.png";
}
