#nullable enable
using System.Diagnostics.CodeAnalysis;
using GeneralTweaks.Components;
using HarmonyLib;
using UnityEngine;

namespace GeneralTweaks.Patches;

public static class MainMenuPatches
{
    [HarmonyPatch(typeof(MainMenu), "Start")]
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InvertIf")]
    private static void AfterStart()
    {
        if (GeneralTweaksPlugin.ConfigFullscreenDisabled.Value)
        {
            Screen.fullScreen = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        if (GeneralTweaksPlugin.ConfigNoPlayGames.Value)
        {
            GameObject? button = GameObject.Find("AchievementsOrLogIn");
            
            if (button == null)
                GeneralTweaksPlugin._Logger.LogWarning("Unable to find Play Games button, can't disable!");
            else
            {
                button.SetActive(false);
            }
        }
    }
    
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.ClickEndless))]
    [HarmonyPostfix]
    private static void AfterClickEndless()
    {
        if (!GeneralTweaksPlugin.ConfigLegacyLeaderboard.Value) return;
        
        GameObject leaderboard = GameObject.Find("LeaderboardBG");

        if (leaderboard.GetComponent<LegacyLeaderboardInjector>() == null)
            leaderboard.AddComponent<LegacyLeaderboardInjector>();
    }
}