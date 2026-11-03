using System.Collections;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

namespace NoRandomPauses
{
    internal static class PauseDebounce
    {
        private sealed class PendingCheck
        {
            public bool HmdUnmounted;
            public bool FocusCaptured;
        }

        private static readonly ConditionalWeakTable<PauseController, PendingCheck> Pending =
            new ConditionalWeakTable<PauseController, PendingCheck>();

        internal static bool Intercept(PauseController controller, IVRPlatformHelper vrPlatformHelper, bool hmdUnmounted)
        {
            PluginConfig config = PluginConfig.Instance;
            if (config == null || !config.Enabled || config.SuppressionMs <= 0 || vrPlatformHelper == null
                || !controller.isActiveAndEnabled)
            {
                return true;
            }
            if (!Pending.TryGetValue(controller, out PendingCheck check))
            {
                check = new PendingCheck();
                Pending.Add(controller, check);
                controller.StartCoroutine(PauseIfStillLost(controller, vrPlatformHelper, check, config.SuppressionMs));
            }
            if (hmdUnmounted)
            {
                check.HmdUnmounted = true;
            }
            else
            {
                check.FocusCaptured = true;
            }
            return false;
        }

        private static IEnumerator PauseIfStillLost(PauseController controller, IVRPlatformHelper vrPlatformHelper,
            PendingCheck check, int periodMs)
        {
            yield return new WaitForSecondsRealtime(periodMs / 1000f);
            Pending.Remove(controller);
            bool headsetOff = check.HmdUnmounted && !vrPlatformHelper.isHmdMounted && !controller.ignoreHMDUUnmountEvets;
            bool focusLost = check.FocusCaptured && !vrPlatformHelper.hasInputFocus;
            if (headsetOff || focusLost)
            {
                Plugin.Log.Info("paused");
                controller.Pause();
            }
            else
            {
                Plugin.Log.Info("suppressed");
            }
        }
    }

    [HarmonyPatch(typeof(PauseController), "HandleHMDUnmounted")]
    internal static class HandleHmdUnmountedPatch
    {
        private static bool Prefix(PauseController __instance, IVRPlatformHelper ____vrPlatformHelper)
        {
            return PauseDebounce.Intercept(__instance, ____vrPlatformHelper, hmdUnmounted: true);
        }
    }

    [HarmonyPatch(typeof(PauseController), "HandleFocusWasCaptured")]
    internal static class HandleFocusWasCapturedPatch
    {
        private static bool Prefix(PauseController __instance, IVRPlatformHelper ____vrPlatformHelper)
        {
            return PauseDebounce.Intercept(__instance, ____vrPlatformHelper, hmdUnmounted: false);
        }
    }
}
