using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModButton.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MainMenuPatches
{
    [CanBeNull] private static GameObject _modsButton;

    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    [HarmonyPostfix]
    private static void AddModsButton(MainMenu __instance)
    {
        // Find statistics button and duplicate it
        GameObject statsButton = GameObject.Find("STATS");
        GameObject modsButton = Object.Instantiate(statsButton, __instance.transform, true);

        _modsButton = modsButton;
        
        Vector3 pos = modsButton.transform.position;
        pos.y -= 117.6f; // Distance between play and customization buttons
        modsButton.transform.position = pos;
        
        modsButton.name = "MODS";
        // We don't have TMP available to us from a plugin, so it's time to do some funky things
        GameObject textMesh = GameObject.Find("MODS/Text (TMP)");
        Component component = textMesh.GetComponentByName("TMPro.TextMeshProUGUI");
        component.GetType().GetProperty("text")!.SetValue(component, modsButton.name);
    }

    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.ClickEndless))]
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.OpenSkins))]
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Settings))]
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Statistics))]
    [HarmonyPrefix]
    private static void HideModsButton()
    {
        if(_modsButton != null) _modsButton.SetActive(false);
    }

    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.backToMainMenu))]
    [HarmonyPostfix]
    private static void RestoreModsButton()
    {
        if(_modsButton != null) _modsButton.SetActive(true);
    }
}