using System.Collections.Generic;
using GameNetcodeStuff;
using UnityEngine;

namespace NearHorizon;

internal static class HorizonFarClip
{
    internal static void SyncLocalPlanes()
    {
        var sor = StartOfRound.Instance;
        if (sor == null) return;

        float d = Plugin.ResolvedFarClip();
        foreach (Camera cam in ViewCameras.Local(sor))
            cam.farClipPlane = d;

        if (Plugin.DebugLogging.Value)
            Debug.Log($"[NearHorizon] farClipPlane={d}");
    }

    private static class ViewCameras
    {
        internal static IEnumerable<Camera> Local(StartOfRound sor)
        {
            PlayerControllerB local = sor.localPlayerController;
            if (local?.gameplayCamera != null)
                yield return local.gameplayCamera;

            if (sor.spectateCamera != null)
                yield return sor.spectateCamera;
        }
    }
}
