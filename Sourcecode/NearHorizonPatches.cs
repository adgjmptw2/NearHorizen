using GameNetcodeStuff;
using HarmonyLib;

namespace NearHorizon;

[HarmonyPatch(typeof(PlayerControllerB), "Start")]
internal static class NearHooks_PlayerStartFarClip
{
    static void Postfix(PlayerControllerB __instance)
    {
        if (StartOfRound.Instance == null) return;
        if (__instance != StartOfRound.Instance.localPlayerController) return;
        HorizonFarClip.SyncLocalPlanes();
    }
}

[HarmonyPatch(typeof(GrabbableObject), "Start")]
[HarmonyPriority(Priority.First)]
internal static class NearHooks_GrabbableStartPropShadow
{
    static void Postfix(GrabbableObject __instance)
    {
        PropShadowTweaks.OnGrabbableStart(__instance);
    }
}
