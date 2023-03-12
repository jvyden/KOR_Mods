using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModButton.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MainMenuPatches
{
    [CanBeNull] private static GameObject _modsButton;

    private static void HandleModsButtonClick()
    {
        Console.WriteLine("REAL");
    }

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
        Component textMeshComponent = textMesh.GetComponentByName("TMPro.TextMeshProUGUI");
        textMeshComponent
            .GetType()
            .GetProperty("text")!
            .SetValue(textMeshComponent, modsButton.name);
        
        // Change button behaviour. Once again, we don't have the button type (UnityEngine.UI.Button) so prepare for hell.
        // This spaghetti is even worse. I spent hours on this. I'm sorry.
        Component buttonComponent = modsButton.GetComponentByName("Button");
        
        FieldInfo onClickInfo = buttonComponent
            .GetType()
            .GetField("m_OnClick", BindingFlags.Instance | BindingFlags.NonPublic);
        
        object onClick = onClickInfo!.GetValue(buttonComponent);
        Type onClickType = onClick.GetType();
        
        // Remove all existing events
        FieldInfo persistentInfo = onClickType.BaseType!.BaseType!
            .GetField("m_PersistentCalls", BindingFlags.Instance | BindingFlags.NonPublic);

        persistentInfo.SetValue(onClick, Activator.CreateInstance(persistentInfo.FieldType));

        onClickType
            .GetMethod("RemoveAllListeners", BindingFlags.Instance | BindingFlags.Public)!
            .Invoke(onClick, Array.Empty<object>());

        // Get the method we're going to invoke with
        MethodInfo invokeMethod = typeof(MainMenuPatches)
            .GetMethod(nameof(HandleModsButtonClick), BindingFlags.Static | BindingFlags.NonPublic);

        // Add our own listener
        onClickType
            .GetMethod("AddListener", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(onClick, new object[]{ null, invokeMethod });
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