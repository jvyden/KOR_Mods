using BepInEx;
using BepInEx.Configuration;
using CheatTools.Patches;
using HarmonyLib;

namespace CheatTools;

[BepInProcess("K.O.R")]
[BepInPlugin("KOR_Mods.CheatTools", "K.O.R Cheat Tools", "1.0.0")]
public class CheatToolsPlugin : BaseUnityPlugin
{
    private Harmony _harmony;

    internal static CheatToolsPlugin Instance;

    public ConfigEntry<bool> ConfigInvincibility;
    public ConfigEntry<bool> ConfigBouncy;
    
    // disable battery colliders so they just pile up on the floor

    private void Awake()
    {
        Instance = this;
        
        this.ConfigInvincibility = Config.Bind("Cheats", "Invincibility", false);
        this.ConfigBouncy = Config.Bind("Cheats", "Bouncy", false);
        
        Config.SaveOnConfigSet = true;
        Config.Save();
    }

    private void Start()
    {
        this._harmony = new Harmony("KOR_Mods.CheatTools");
        this._harmony.PatchAll(typeof(HealthSystemPatches));
    }

    private void OnDestroy()
    {
        this._harmony.UnpatchSelf();
    }
}