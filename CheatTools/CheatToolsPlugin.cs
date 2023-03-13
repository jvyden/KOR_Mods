using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using CheatTools.Patches;
using HarmonyLib;
using UnityEngine;

namespace CheatTools;

[BepInProcess("K.O.R")]
[BepInPlugin("KOR_Mods.CheatTools", "K.O.R Cheat Tools", "1.0.1")]
public class CheatToolsPlugin : BaseUnityPlugin
{
    private Harmony _harmony;

    public static ConfigEntry<bool> ConfigInvincibility;
    public static ConfigEntry<bool> ConfigBouncy;

    private readonly Lazy<CosmeticManager[]> _cosmeticManagers = new(FindObjectsOfType<CosmeticManager>);

    // disable battery colliders so they just pile up on the floor

    private void Awake()
    {
        ConfigInvincibility = Config.Bind("Player", "Invincibility", false);
        ConfigBouncy = Config.Bind("Fun", "Bouncy Robot", false);

        Config.Bind("Coins", "Add coins", string.Empty, new ConfigDescription(string.Empty, null, new ConfigurationManagerAttributes()
        {
            CustomDrawer = AddCoinsDrawer
        }));
        
        Config.Bind("Cosmetics", "Unlock", string.Empty, new ConfigDescription(string.Empty, null, new ConfigurationManagerAttributes()
        {
            CustomDrawer = UnlockAllCosmeticsDrawer
        }));
        
        Config.SaveOnConfigSet = true;
        Config.Save();
    }
    
    private void AddCoinsDrawer(ConfigEntryBase entry)
    {
        if (GUILayout.Button("Add 100 coins", GUILayout.ExpandWidth(true)))
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + 100);
        }
        if (GUILayout.Button("Add 1000 coins", GUILayout.ExpandWidth(true)))
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + 1000);
        }
    }

    private void UnlockAllCosmeticsDrawer(ConfigEntryBase entry)
    {
        foreach (CosmeticManager cosmeticManager in _cosmeticManagers.Value)
        {
            if (GUILayout.Button($"Unlock all {cosmeticManager.typeOfCosmetic}s", GUILayout.ExpandWidth(true)))
                UnlockAllCosmetics(cosmeticManager);
        }
    }

    private static void UnlockAllCosmetics(CosmeticManager cosmeticManager)
    {
        for (var i = 0; i < cosmeticManager.cosmetics.Length; i++)
        {
            CosmeticItem cosmetic = cosmeticManager.cosmetics[i];
            Console.WriteLine($"Legally obtaining {cosmeticManager.typeOfCosmetic} cosmetic {cosmetic.AnimationName} (order: {i})");
            PlayerPrefs.SetInt(cosmeticManager.typeOfCosmetic + cosmeticManager.Cosmetics[i].AnimationName, 1);
        }
    }

    private void Start()
    {
        this._harmony = new Harmony("KOR_Mods.CheatTools");
        this._harmony.PatchAll(typeof(HealthSystemPatches));
        this._harmony.PatchAll(typeof(CosmeticsUIPatches));
        this._harmony.PatchAll(typeof(MovementScriptPatches));
    }

    private void OnDestroy()
    {
        this._harmony.UnpatchSelf();
    }
}