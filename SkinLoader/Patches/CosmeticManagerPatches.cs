using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using SkinLoader.Helpers;

namespace SkinLoader.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class CosmeticManagerPatches
{
    private static readonly MethodInfo AddCustomSkinsInfo = SymbolExtensions.GetMethodInfo(() => SkinHelper.AddCustomSkins(null));
    private static readonly FieldInfo CosmeticsField = AccessTools.Field(typeof(CosmeticManager), "cosmetics");
    
    [HarmonyPatch(typeof(CosmeticManager), "LoadSkinsFromFolder")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> StartTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        var found = false;
        var patched = false;
        foreach (var instruction in instructions)
        {
            if (instruction.StoresField(CosmeticsField))
            {
                found = true;
                yield return instruction;
                continue;
            }

            if (found && !patched)
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Call, AddCustomSkinsInfo);
                patched = true;
            }
            yield return instruction;
        }
        if (found is false)
            throw new Exception("Cannot find <Stdfld cosmetics> in CosmeticManager.LoadSkinsFromFolder");
    }
}