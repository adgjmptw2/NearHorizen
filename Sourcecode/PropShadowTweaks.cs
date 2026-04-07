using System.Collections.Generic;
using UnityEngine;

namespace NearHorizon;

internal static class PropShadowTweaks
{
    private static readonly Dictionary<string, LightShadows> Defaults = new();

    internal static void OnGrabbableStart(GrabbableObject g) => ApplyTo(g);

    internal static void OnStripSettingChanged()
    {
        foreach (var g in Object.FindObjectsByType<GrabbableObject>(FindObjectsSortMode.None))
            ApplyTo(g);
    }

    private static void ApplyTo(GrabbableObject g)
    {
        var ip = g.itemProperties;
        if (ip == null) return;
        string n = ip.name;
        if (n != "FancyLamp" && n != "LungApparatus") return;

        var light = g.GetComponentInChildren<Light>();
        if (light == null) return;

        if (!Defaults.ContainsKey(n))
            Defaults[n] = light.shadows;

        if (Plugin.StripLampLungShadows.Value)
            light.shadows = LightShadows.None;
        else
            light.shadows = Defaults[n];
    }
}
