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

    private ConfigEntry<bool> _configInvincibility;
    public bool ConfigInvincibility => _configInvincibility.Value;

    private void Awake()
    {
        Instance = this;
        
        this._configInvincibility = Config.Bind("Cheats", "Invincibility", false, "xd");
        
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