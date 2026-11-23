using Godot;

namespace ShoreMagician.Code.Extensions;

public static class StringExtensions
{
    private static string ImgPath(string subfolder, string file) =>
        $"{ShoreMagicianMainFile.ModId}/images/{subfolder}/{file}";

    private static string WithFallback(string subfolder, string file, string fallbackFile)
    {
        var primary = ImgPath(subfolder, file);
        return ResourceLoader.Exists($"res://{primary}")
            ? $"res://{primary}"
            : $"res://{ImgPath(subfolder, fallbackFile)}";
    }

    public static string CardImagePath(this string file) =>
        WithFallback("cards", file, "todo.png");

    public static string PowerImagePath(this string file) =>
        WithFallback("powers", file, "todo_power.png");

    public static string BigPowerImagePath(this string file) =>
        WithFallback("powers", file, "todo_power_big.png");

    public static string BigRelicImagePath(this string file) =>
        WithFallback("relics", file, "todo_relic_big.png");

    public static string RelicImagePath(this string file) =>
        WithFallback("relics", file, "todo_relic.png");
}
