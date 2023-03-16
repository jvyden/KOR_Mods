using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace CheatTools.Patches;

public static class GameManagerPatcher
{
    [HarmonyPatch(typeof(GameManager), "Update")]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> StartTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        if (!CheatToolsPlugin.ConfigNoSpeedCap.Value)
        {
            foreach (CodeInstruction i in instructions)
            {
                yield return i;
            }

            yield break;
        }
        
        var found = false;
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Ldc_R4 && !found)
            {
                found = true;
                yield return new CodeInstruction(OpCodes.Ldc_R4, float.MaxValue);
                continue;
            }
            
            yield return instruction;
        }
        if (found is false)
            throw new Exception("Cannot find <Stdfld cosmetics> in CosmeticManager.LoadSkinsFromFolder");
    }
}