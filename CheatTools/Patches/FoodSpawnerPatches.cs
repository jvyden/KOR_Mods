using HarmonyLib;
using Scripts.FallingObjects;

namespace CheatTools.Patches;

public static class FoodSpawnerPatches
{
    [HarmonyPatch(typeof(FoodSpawner), "SpawnSlowMo")]
    [HarmonyPrefix]
    public static bool ShouldSpawnSlowMo()
    {
        return !CheatToolsPlugin.ConfigNoSlow.Value;
    }
    
    [HarmonyPatch(typeof(FoodSpawner), "SpawnHeal")]
    [HarmonyPrefix]
    public static bool ShouldSpawnHeal()
    {
        return !CheatToolsPlugin.ConfigNoHeal.Value;
    }
    
    [HarmonyPatch(typeof(FoodSpawner), "SpawnLakrits")]
    [HarmonyPrefix]
    public static bool ShouldSpawnMeteor()
    {
        return !CheatToolsPlugin.ConfigNoMeteors.Value;
    }
}