using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace NearHorizon;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "mine9289.NearHorizon";
    public const string PluginName = "NearHorizon";
    public const string PluginVersion = "1.5.0";

    public static Plugin Instance { get; private set; } = null!;

    public static ConfigEntry<float> FarClipPlane { get; private set; } = null!;
    public static ConfigEntry<bool> DebugLogging { get; private set; } = null!;
    public static ConfigEntry<bool> StripLampLungShadows { get; private set; } = null!;

    internal const float BadClipFallback = 200f;

    private Harmony harmony = null!;

    private void Awake()
    {
        Instance = this;
        FarClipPlane = Config.Bind(
            "General",
            "FarClipPlane",
            120f,
            "View distance in units. Vanilla is 400. (between 1 and 10000)");
        DebugLogging = Config.Bind("General", "DebugLogging", false, "Whether to display debug messages.");
        StripLampLungShadows = Config.Bind(
            "General",
            "StripLampLungShadows",
            true,
            "Fancy lamp & lung apparatus: Light shadows off");

        CoerceFarClipIfBad();
        FarClipPlane.SettingChanged += (_, __) =>
        {
            CoerceFarClipIfBad();
            HorizonFarClip.SyncLocalPlanes();
        };
        StripLampLungShadows.SettingChanged += (_, __) => PropShadowTweaks.OnStripSettingChanged();

        harmony = new Harmony("mine9289.nearhorizon");
        harmony.PatchAll(typeof(Plugin).Assembly);

        Logger.LogInfo("mine9289 / NearHorizon " + PluginVersion);
    }

    internal static void CoerceFarClipIfBad()
    {
        float v = FarClipPlane.Value;
        if (float.IsNaN(v) || v < 1f || v > 10000f)
            FarClipPlane.Value = BadClipFallback;
    }

    internal static float ResolvedFarClip()
    {
        float v = FarClipPlane.Value;
        return float.IsNaN(v) || v < 1f || v > 10000f ? BadClipFallback : v;
    }
}
