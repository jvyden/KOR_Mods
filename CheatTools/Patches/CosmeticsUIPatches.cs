using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;

namespace CheatTools.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class CosmeticsUIPatches
{
    private static readonly FieldInfo _uiForWhatInfo = AccessTools.Field(typeof(CosmeticsUI), "UIForWhat");
    private static readonly FieldInfo _cosmeticManagerInfo = AccessTools.Field(typeof(CosmeticsUI), "cosmeticManager");
    
    [HarmonyPatch(typeof(CosmeticsUI), nameof(CosmeticsUI.Buy))]
    [HarmonyPrefix]
    public static void BeforeBuy(CosmeticsUI __instance)
    {
        string uiForWhat = (string)_uiForWhatInfo.GetValue(__instance);
        CosmeticManager cosmeticManager = (CosmeticManager)_cosmeticManagerInfo.GetValue(__instance);
        
        Console.WriteLine($"Buying {uiForWhat} cosmetic id {cosmeticManager.Index}");
    }
}