using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ExtraStatistics.Patches;
using HarmonyLib;

namespace ExtraStatistics
{
    [BepInPlugin("KOR_MODS.ExtraStatistics", "Extra Statistics", "1.0.0")]
    public class ExtraStatisticsPlugin : BaseUnityPlugin
    {
        private Harmony _harmony;

        // public static ManualLogSource _Logger;

        public static ConfigEntry<bool> ConfigEnableMisses;

        private void Awake()
        {
            ConfigEnableMisses = Config.Bind("Trackers", "EnableMisses", false);
        }

        private void Start()
        {
            this._harmony = new Harmony("KOR_MODS.ExtraStatistics");
            this._harmony.PatchAll(typeof(MovementScriptPatches));
            this._harmony.PatchAll(typeof(SurDodSkallePatches));
        }
    }
}
