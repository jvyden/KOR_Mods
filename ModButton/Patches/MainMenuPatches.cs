using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using Logger = BepInEx.Logging.Logger;

namespace ModButton.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MainMenuPatches
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.ClickEndless))]
    [HarmonyPrefix]
    private static bool ClickEndless(MainMenu __instance)
    {
        __instance.StartCoroutine(AudioHelper.LoadAndPlayFile(__instance, @"Z:\home\jvyden\Downloads\fixed_offkey_shit_at_the_end.mp3"));
        return false;
    }
}