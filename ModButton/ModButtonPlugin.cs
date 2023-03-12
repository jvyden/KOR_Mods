using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModButton.Patches;

namespace ModButton;

[BepInPlugin("KOR_Mods.ModButton", "Mod Button", "1.0.0")]
public class ModButtonPlugin : BaseUnityPlugin
{
    private Harmony _harmony;
    
    // ReSharper disable once InconsistentNaming
    internal static ManualLogSource _Logger;
        
    private void Start()
    {
        this._harmony = new Harmony("KOR_Mods.ModButton");
        this._harmony.PatchAll(typeof(MainMenuPatches));
        
        _Logger = this.Logger;
        
        Logger.LogInfo("Successfully loaded and patched in the mod button!");
    }

    private void OnDestroy()
    {
        this._harmony.UnpatchSelf();
    }
}