#nullable enable
using System.Diagnostics.CodeAnalysis;
using GeneralTweaks.Components;
using HarmonyLib;
using UnityEngine;

namespace GeneralTweaks.Patches;

public static class MainMenuPatches
{
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