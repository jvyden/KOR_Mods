using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ExtraStatistics.Statistics;
using ExtraStatistics.Statistics.Trackers;
using HarmonyLib;
using UnityEngine;

namespace ExtraStatistics.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MovementScriptPatches
{
    [HarmonyPatch(typeof(MovementScript), "Awake")]
    [HarmonyPostfix]
    public static void AfterAwake(MovementScript __instance)
    {
        MissTracker.Misses = 0;
        _yOffset = 1; // Since we're copying from WipeCounter
        
        GameObject wipeCounter = GameObject.Find("WipeCounter");

        if(ExtraStatisticsPlugin.ConfigEnableMisses.Value) CreateStatistic<MissTracker>(wipeCounter);
    }

    private static int _yOffset = 0;

    private static void CreateStatistic<TTracker>(GameObject wipeCounter) where TTracker : Tracker
    {
        // duplicate wipe counter text
        GameObject statisticObj = Object.Instantiate(wipeCounter, wipeCounter.transform.parent, true);
        
        // offset our new text
        Vector3 newPos = statisticObj.transform.localPosition;
        newPos.y -= 28 * _yOffset++;
        statisticObj.transform.localPosition = newPos;
        
        // nuke vanilla tracker
        Object.Destroy(statisticObj.GetComponent<WipesText>());
        
        // add our own statistic and tracker
        statisticObj.AddComponent<Statistic>();
        statisticObj.AddComponent<TTracker>();
    }
}