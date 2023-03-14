using BepInEx;
using BepInEx.Configuration;
using ExtraStatistics.Patches;
using HarmonyLib;

namespace ExtraStatistics
{
    [BepInPlugin("KOR_MODS.ExtraStatistics", "Extra Statistics", "1.0.1")]
    public class ExtraStatisticsPlugin : BaseUnityPlugin
    {
        private Harmony _harmony;

        // public static ManualLogSource _Logger;

        public static ConfigEntry<bool> ConfigEnableMisses;
        public static ConfigEntry<bool> ConfigEnableCoins;

        private void Awake()
        {
            ConfigEnableMisses = Config.Bind("Trackers", "Show Misses", false);
            ConfigEnableCoins = Config.Bind("Trackers", "Show Earned Coins", false);
        }

        private void Start()
        {
            this._harmony = new Harmony("KOR_MODS.ExtraStatistics");
            this._harmony.PatchAll(typeof(MovementScriptPatches));
            this._harmony.PatchAll(typeof(SurDodSkallePatches));
        }
    }
}
