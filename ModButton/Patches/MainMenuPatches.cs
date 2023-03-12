using HarmonyLib;

namespace ModButton.Patches;

public class MainMenuPatches
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.ClickEndless))]
    [HarmonyPrefix]
    private static bool ClickEndless()
    {
        return false;
    }
}