using BepInEx;
using HarmonyLib;
using ModButton.Patches;

namespace ModButton;

[BepInPlugin("KOR_Mods.ModButton", "Mod Button", "1.0.0")]
public class ModButtonPlugin : BaseUnityPlugin
{
    private Harmony _harmony;
        
    private void Start()
    {
        this._harmony = new Harmony("KOR_Mods.ModButton");
        this._harmony.PatchAll(typeof(MainMenuPatches));
        
        Logger.LogInfo("Successfully loaded and patched in the mod button!");
    }

    private void OnDestroy()
    {
        this._harmony.UnpatchSelf();
    }
}