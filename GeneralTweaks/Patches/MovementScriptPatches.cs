using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

namespace GeneralTweaks.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MovementScriptPatches
{
    [HarmonyPatch(typeof(MovementScript), "Awake")]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InvertIf")]
    public static void AfterAwake(MovementScript __instance)
    {
        if (GeneralTweaksPlugin.ConfigFixTrailOrder.Value)
        {
            TrailRenderer renderer = __instance.GetComponentInChildren<TrailRenderer>();
            renderer.sortingLayerName = "Default";
        }
    }
}