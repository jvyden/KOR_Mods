using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModButton.Patches;

namespace ModButton;

[BepInProcess("K.O.R")]
[BepInPlugin("KOR_Mods.ModButton", "Mod Button", "1.0.1")]
[BepInDependency(ConfigurationManager.ConfigurationManager.GUID)]
public class ModButtonPlugin : BaseUnityPlugin
{
    private Harmony _harmony;
    
    // ReSharper disable InconsistentNaming
    internal static ManualLogSource _Logger;
    internal static ConfigurationManager.ConfigurationManager ConfigManager;
    // ReSharper restore InconsistentNaming
        
    private void Start()
    {
        this._harmony = new Harmony("KOR_Mods.ModButton");
        this._harmony.PatchAll(typeof(MainMenuPatches));
        
        _Logger = this.Logger;

        ConfigManager = GetComponent<ConfigurationManager.ConfigurationManager>();
        ConfigManager.OverrideHotkey = true;
        
        Logger.LogInfo("Successfully loaded and patched in the mod button!");
    }

    private void OnDestroy()
    {
        this._harmony.UnpatchSelf();
    }
}