using ExtraStatistics.Statistics.Trackers;
using HarmonyLib;
using Scripts.FallingObjects;

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