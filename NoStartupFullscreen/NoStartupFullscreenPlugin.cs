using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using NoStartupFullscreen.Patches;

namespace NoStartupFullscreen
{
    [BepInPlugin("KOR_Mods.NoStartupFullscreen", "No Startup Fullscreen", "1.0.0")]
    public class NoStartupFullscreenPlugin : BaseUnityPlugin
    {
        private Harmony _harmony;

        public static ConfigEntry<bool> ConfigEnabled;

        private void Awake()
        {
            ConfigEnabled = Config.Bind("Config", "Enabled", true);
        }

        private void Start()
        {
            this._harmony = new Harmony("KOR_Mods.NoStartupFullscreen");
            if (!ConfigEnabled.Value) return;
            
            this._harmony.PatchAll(typeof(StartCounterPatches));
            this._harmony.PatchAll(typeof(MainMenuPatches));
        }

        private void OnDestroy()
        {
            this._harmony.UnpatchSelf();
        }
    }
}
