using BepInEx;
using BepInEx.Configuration;
using FPSCounter.Patches;
using HarmonyLib;

namespace FPSCounter
{
    [BepInPlugin("KOR_Mods.FPSCounter", "FPS Counter", "1.0.0")]
    public class FPSCounterPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> ConfigEnableFpsCounter;
        
        private Harmony _harmony;

        private void Awake()
        {
            ConfigEnableFpsCounter = Config.Bind("Config", "Enabled", false);
            
            Config.SaveOnConfigSet = true;
            Config.Save();
        }

        private void Start()
        {
            this._harmony = new Harmony("KOR_Mods.CheatTools");
            this._harmony.PatchAll(typeof(GameManagerPatches));
        }
    }
}
