using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;

namespace ShoreMagician.Code.Patches;

public sealed class ModPatcher
{
    private readonly Harmony _harmony;
    private readonly Logger _logger;
    private readonly List<Type> _types = [];
    private bool _applied;

    private ModPatcher(string modId, Logger logger)
    {
        _harmony = new Harmony(modId);
        _logger = logger;
    }

    public static ModPatcher Create(string modId, Logger logger) => new(modId, logger);

    public ModPatcher AddAllFrom(Assembly assembly)
    {
        var found = SafeGetTypes(assembly)
            .Where(t => t.GetCustomAttributes(typeof(HarmonyPatch), inherit: false).Length > 0)
            .ToList();
        _types.AddRange(found);
        return this;
    }

    public void PatchAll()
    {
        if (_applied) return;
        _applied = true;

        foreach (var type in _types)
        {
            try
            {
                _harmony.CreateClassProcessor(type).Patch();
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed patching {type.FullName}: {ex}");
            }
        }
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null)!;
        }
    }
}
