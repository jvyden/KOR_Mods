using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GeneralTweaks.Patches;
using HarmonyLib;

namespace GeneralTweaks
{
    [BepInPlugin("KOR_Mods.GeneralTweaks", "General K.O.R Tweaks", "1.0.1")]
    public class GeneralTweaksPlugin : BaseUnityPlugin
    {
        private Harmony _harmony;

        public static ConfigEntry<bool> ConfigFullscreenDisabled;
        public static ConfigEntry<bool> ConfigNoPlayGames;
        public static ConfigEntry<bool> ConfigFixTrailOrder;

        // ReSharper disable once InconsistentNaming
        public static ManualLogSource _Logger;

        private void Awake()
        {
            _Logger = this.Logger;
            
            ConfigFullscreenDisabled = Config.Bind("Tweaks", "No startup fullscreen", true);
            ConfigNoPlayGames = Config.Bind("Tweaks", "Remove Google Play Games button", true);
            
            ConfigFixTrailOrder = Config.Bind("Fixes", "Fix trail layer", true, 
                "By default, the trail is set to the wrong sortingLayer causing it to disappear behind backgrounds like Mountains." +
                " This fixes that.");
        }

        private void Start()
        {
            this._harmony = new Harmony("KOR_Mods.GeneralTweaks");

            this._harmony.PatchAll(typeof(StartCounterPatches));
            this._harmony.PatchAll(typeof(MainMenuPatches));
            this._harmony.PatchAll(typeof(MovementScriptPatches));
        }

        private void OnDestroy()
        {
            this._harmony.UnpatchSelf();
        }
    }
}
