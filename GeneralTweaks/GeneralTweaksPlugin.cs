using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GeneralTweaks.Patches;
using HarmonyLib;

namespace GeneralTweaks
{
    [BepInPlugin("KOR_Mods.GeneralTweaks", "K.O.R General Tweaks", "1.0.2")]
    public class GeneralTweaksPlugin : BaseUnityPlugin
    {
        private Harmony _harmony;
        
        public static ConfigEntry<bool> ConfigLegacyLeaderboard;

        // ReSharper disable once InconsistentNaming
        public static ManualLogSource _Logger;

        private void Awake()
        {
            _Logger = this.Logger;
            
            ConfigLegacyLeaderboard = Config.Bind("Tweaks", "Use legacy leaderboard", false, 
                "K.O.R 1.1 used a different system for leaderboards. Enable this to use that leaderboard.");
        }

        private void Start()
        {
            this._harmony = new Harmony("KOR_Mods.GeneralTweaks");
            
            this._harmony.PatchAll(typeof(MainMenuPatches));
        }

        private void OnDestroy()
        {
            this._harmony.UnpatchSelf();
        }
    }
}
