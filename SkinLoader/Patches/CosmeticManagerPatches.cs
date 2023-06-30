using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using Cosmetics;
using HarmonyLib;
using SkinLoader.Helpers;

namespace SkinLoader.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class CosmeticManagerPatches
{
    // placeholder, don't actually use
    private static CosmeticItem[] cosmetics = Array.Empty<CosmeticItem>();
    
    private static readonly MethodInfo AddCustomSkinsInfo = SymbolExtensions.GetMethodInfo(() => SkinHelper.AddCustomSkins(0, ref cosmetics));
    private static readonly MethodInfo CalculateOwnedSkinsInfo = AccessTools.Method(typeof(CosmeticManager), nameof(CosmeticManager.CalculateOwnedCosmetics));
    
    [HarmonyPatch(typeof(CosmeticManager), "LoadSkins")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> StartTranspilerSkins(IEnumerable<CodeInstruction> instructions)
    {
        bool found = false;
        bool patched = false;
        foreach (var instruction in instructions)
        {
            if (instruction.Calls(CalculateOwnedSkinsInfo))
            {
                found = true;
                yield return instruction;
                continue;
            }

            if (found && !patched)
            {
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return CodeInstruction.LoadField(typeof(CosmeticManager), nameof(CosmeticManager.skins), true);
                yield return new CodeInstruction(OpCodes.Call, AddCustomSkinsInfo);
                patched = true;
            }
            yield return instruction;
        }
        if (found is false)
            throw new Exception("Cannot find <Stdfld cosmetics> in CosmeticManager.LoadSkins");
    }
    
    [HarmonyPatch(typeof(CosmeticManager), "LoadBackgrounds")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> StartTranspilerBackgrounds(IEnumerable<CodeInstruction> instructions)
    {
        bool found = false;
        bool patched = false;
        foreach (var instruction in instructions)
        {
            if (instruction.Calls(CalculateOwnedSkinsInfo))
            {
                found = true;
                yield return instruction;
                continue;
            }

            if (found && !patched)
            {
                yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                yield return CodeInstruction.LoadField(typeof(CosmeticManager), nameof(CosmeticManager.backgrounds), true);
                yield return new CodeInstruction(OpCodes.Call, AddCustomSkinsInfo);
                patched = true;
            }
            yield return instruction;
        }
        if (found is false)
            throw new Exception("Cannot find <Stdfld cosmetics> in CosmeticManager.LoadBackgrounds");
    }
}