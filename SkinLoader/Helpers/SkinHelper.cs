using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using BepInEx;
using Cosmetics;
using HarmonyLib;
using UnityEngine;

namespace SkinLoader.Helpers;

[SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
public static class SkinHelper
{
    private static readonly Dictionary<CosmeticType, CosmeticItem[]> CustomCosmeticCache = new();

    public static void AddCustomSkins(CosmeticType type, ref CosmeticItem[] cosmetics)
    {
        if (CustomCosmeticCache.TryGetValue(type, out CosmeticItem[] cachedItems))
        {
            cosmetics = cosmetics.AddRangeToArray(cachedItems);
            return;
        }

        SkinLoaderPlugin._Logger.LogInfo($"Loading custom '{type}' cosmetics");
        int order = cosmetics.Max(c => c.MenuOrder) + 1;

        string path = Path.Combine(Paths.GameRootPath, type.ToString());

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        string[] files = Directory.GetFiles(path);
        
        bool isSkin = type == CosmeticType.Skin;
        bool isBackground = type == CosmeticType.Background;
        
        Vector2 textureSize = isSkin ? new Vector2(16, 16) : new Vector2(176, 128);

        List<CosmeticItem> customItems = new();
        foreach (string filename in files)
        {
            CosmeticItem customItem = ScriptableObject.CreateInstance<CosmeticItem>();
            
            SkinLoaderPlugin._Logger.LogInfo($"Loading {type} at {filename} (order: {order})");

            Texture2D tex = isSkin ? new Texture2D(256, 256) : new Texture2D(1920, 1080);
            tex.filterMode = FilterMode.Point;

            tex.LoadImage(File.ReadAllBytes(filename));

            SkinLoaderPlugin._Logger.LogDebug($"Real skin resolution: {tex.width}x{tex.height}");
            SkinLoaderPlugin._Logger.LogDebug($"Base skin resolution: {textureSize.x}x{textureSize.y}");
            
            Rect sizeRect = new Rect(0, 0, tex.width, tex.height);

            bool scaleFactorIsWidth = tex.width < tex.height;
            float scale = 16f;

            if (scaleFactorIsWidth) scale *= tex.width / textureSize.x;
            else scale *= tex.height / textureSize.y;
            
            SkinLoaderPlugin._Logger.LogDebug($"Pixel scale: {scale}");
            
            // Done scaling logic, actually create cosmetic now

            customItem.Icon = Sprite.Create(tex, sizeRect, new Vector2(0.5f, 0.5f), scale);

            AccessTools.Field(typeof(CosmeticItem), "menuOrder").SetValue(customItem, order);
            AccessTools.Field(typeof(CosmeticItem), "price").SetValue(customItem, 0);

            string name = Path.GetFileNameWithoutExtension(filename);
            AccessTools.Field(typeof(CosmeticItem), "cosmeticName").SetValue(customItem, "mod - " + name);
            AccessTools.Field(typeof(CosmeticItem), "animationName").SetValue(customItem, "skinLoaderMod" + name);

            customItems.Add(customItem);
            order++;
        }

        CosmeticItem[] customItemsArr = customItems.ToArray();

        CustomCosmeticCache.Add(type, customItemsArr);
        cosmetics = cosmetics.AddRangeToArray(customItemsArr);
        SkinLoaderPlugin._Logger.LogInfo($"Injected {customItems.Count} {type} cosmetics");
    }
}