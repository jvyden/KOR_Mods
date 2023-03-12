using System;
using HarmonyLib;

namespace CheatTools.Patches;

public static class HealthSystemPatches
{
    [HarmonyPatch(typeof(HealthSystem), nameof(HealthSystem.TakeDamage))]
    [HarmonyPrefix]
    private static bool TakeDamagePatch()
    {
        return !CheatToolsPlugin.Instance.ConfigInvincibility;
    } 
}