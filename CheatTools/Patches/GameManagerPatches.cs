using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

namespace CheatTools.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class GameManagerPatches
{
    [HarmonyPatch(typeof(GameManager), "Update")]
    [HarmonyPrefix]
    public static bool BeforeUpdate(GameManager __instance)
    {
        if (!CheatToolsPlugin.ConfigNoSpeedCap.Value && __instance.gameSpeed >= 10.0)
            __instance.gameSpeed = 10f;
        else
            __instance.gameSpeed = (float)(1.75 + __instance.Timer / 15.0);
        
        __instance.Timer += Time.deltaTime;

        return false;
    }
    
    [HarmonyPatch(typeof(GameManager), "UploadDataToServer")]
    [HarmonyPrefix]
    public static bool ShouldSubmitScorePatch()
    {
        return !CheatToolsPlugin.IsCheating;
    } 
}