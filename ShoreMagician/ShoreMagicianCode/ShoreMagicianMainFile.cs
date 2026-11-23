using System.Reflection;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using ShoreMagician.Code.Patches;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace ShoreMagician.Code;

[ModInitializer(nameof(Initialize))]
public static class ShoreMagicianMainFile
{
    public const string ModId = "ShoreMagician";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        ScriptManagerBridge.LookupScriptsInAssembly(assembly);

        var patcher = ModPatcher.Create(ModId, Logger);
        patcher.AddAllFrom(assembly);
        patcher.PatchAll();

        Logger.Info("ShoreMagician mod initialized.");
    }
}
