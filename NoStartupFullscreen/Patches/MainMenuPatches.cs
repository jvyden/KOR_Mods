using HarmonyLib;
using UnityEngine;

namespace NoStartupFullscreen.Patches;

public class MainMenuPatches
{
    [HarmonyPatch(typeof(MainMenu), "Start")]
    [HarmonyPostfix]
    private static void AfterStart()
    {
        Screen.fullScreen = false;
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }
}