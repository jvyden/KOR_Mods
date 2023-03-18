using System;
using BepInEx;
using BepInEx.Configuration;
using CheatTools.Patches;
using HarmonyLib;
using UnityEngine;

namespace CheatTools;

[BepInProcess("K.O.R")]
[BepInPlugin("KOR_Mods.CheatTools", "K.O.R Cheat Tools", "1.0.4")]
public class CheatToolsPlugin : BaseUnityPlugin
{
    private Harmony _harmony;

    // Player
    public static ConfigEntry<bool> ConfigInvincibility;
    public static ConfigEntry<bool> ConfigMouseX;
    public static ConfigEntry<bool> ConfigCircle;
    
    // Fun
    public static ConfigEntry<bool> ConfigBouncy;
    
    // Spawning
    public static ConfigEntry<bool> ConfigNoHeal;
    public static ConfigEntry<bool> ConfigNoSlow;
    public static ConfigEntry<bool> ConfigNoMeteors;
    
    // Game
    public static ConfigEntry<bool> ConfigNoSpeedCap;

    private readonly Lazy<CosmeticManager[]> _cosmeticManagers = new(FindObjectsOfType<CosmeticManager>);

    // disable battery colliders so they just pile up on the floor

    public static bool IsCheating => ConfigInvincibility.Value ||
                                     ConfigBouncy.Value ||
                                     ConfigMouseX.Value ||
                                     ConfigNoHeal.Value ||
                                     ConfigNoSlow.Value ||
                                     ConfigNoSpeedCap.Value ||
                                     ConfigNoMeteors.Value ||
                                     ConfigCircle.Value;

    private void Awake()
    {
        ConfigInvincibility = Config.Bind("Player", "Invincibility", false);
        ConfigMouseX = Config.Bind("Player", "Mouse as X position", false);
        ConfigCircle = Config.Bind("Player", "Circular hitbox", false);
        
        ConfigBouncy = Config.Bind("Fun", "Bouncy Robot", false);

        ConfigNoHeal = Config.Bind("Spawning", "No Heal Batteries", false);
        ConfigNoSlow = Config.Bind("Spawning", "No Slow-mo Batteries", false);
        ConfigNoMeteors = Config.Bind("Spawning", "No Meteors", false);

        ConfigNoSpeedCap = Config.Bind("Game", "No speed cap", false,
            "Stops the game from trying to cap it's speed at 10. Requires restart.");

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
        this._harmony.PatchAll(typeof(FoodSpawnerPatches));
        this._harmony.PatchAll(typeof(GameManagerPatcher));
    }

    private void OnDestroy()
    {
        this._harmony.UnpatchSelf();
    }
}