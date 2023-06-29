using ExtraStatistics.Statistics.Trackers;
using FallingObjects;
using HarmonyLib;

namespace ExtraStatistics.Patches;

public static class SurDodSkallePatches
{
    [HarmonyPatch(typeof(SurDodSkalle), "Floor")]
    [HarmonyPostfix]
    public static void AfterHitFloor()
    {
        MissTracker.Misses++;
    }
}