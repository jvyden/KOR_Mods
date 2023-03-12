using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace NoStartupFullscreen.Patches;

public static class StartCounterPatches
{
    private static readonly PropertyInfo FullscreenField = AccessTools.Property(typeof(Screen), nameof(Screen.fullScreen));
    
    [HarmonyPatch(typeof(StartCounter), "Awake")]
    [HarmonyTranspiler]
    [HarmonyEmitIL]
    private static IEnumerable<CodeInstruction> StartTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        bool found = false;
        int foundIdx = 0;
        foreach (var instruction in instructions)
        {
            // Screen.fullScreen = ...
            // IL_002c: ldc.i4.1
            // IL_002d: call         void [UnityEngine.CoreModule]UnityEngine.Screen::set_fullScreen(bool)
            if (instruction.opcode == OpCodes.Ldc_I4_1 && foundIdx++ == 1) // ... true
            {
                found = true;
                continue;
            }

            if (instruction.Calls(FullscreenField.GetSetMethod()))
            {
                continue;
            }
            
            yield return instruction;
        }
        if (found is false)
            throw new Exception("Cannot find <Stdfld Screen.fullScreen> in StartCounter.Awake");
    }
}