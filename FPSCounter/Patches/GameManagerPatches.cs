using HarmonyLib;
using UnityEngine;

namespace FPSCounter.Patches;

public static class GameManagerPatches
{
    [HarmonyPatch(typeof(GameManager), "Awake")]
    [HarmonyPostfix]
    public static void AfterAwake()
    {
        GameObject fpsCounter = GameObject.Find("/Canvas/SafeShit/FPS Counter");
        fpsCounter.SetActive(FPSCounterPlugin.ConfigEnableFpsCounter.Value);
    }
}