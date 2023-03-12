using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace SkinLoader.Helpers;

[SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
public static class SkinHelper
{
    public static void AddCustomSkins(CosmeticManager cosmeticManager)
    {
        if (cosmeticManager == null) return;
        int order = cosmeticManager.cosmetics.Max(c => c.MenuOrder) + 1;

        string path = Path.Combine(Paths.GameRootPath, cosmeticManager.typeOfCosmetic);

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        string[] files = Directory.GetFiles(path);
        
        Vector2 textureSize = cosmeticManager.typeOfCosmetic == "Skin" ? new Vector2(16, 16) : new Vector2(176, 128);

        List<CosmeticItem> customItems = new();
        foreach (string filename in files)
        {
            CosmeticItem customItem = ScriptableObject.CreateInstance<CosmeticItem>();

            Texture2D tex = new Texture2D(1920, 1080);
            tex.filterMode = FilterMode.Point;

            tex.LoadImage(File.ReadAllBytes(filename));
            
            Rect sizeRect = new Rect(0, 0, textureSize.x, textureSize.y);

            customItem.Icon = Sprite.Create(tex, sizeRect, new Vector2(0.5f, 0.5f), 16f);

            AccessTools.Field(typeof(CosmeticItem), "menuOrder").SetValue(customItem, order);
            AccessTools.Field(typeof(CosmeticItem), "price").SetValue(customItem, 0);

            string name = "mod - " + Path.GetFileNameWithoutExtension(filename);
            AccessTools.Field(typeof(CosmeticItem), "cosmeticName").SetValue(customItem, name);
            
            customItems.Add(customItem);
            order++;
        }
        
        cosmeticManager.cosmetics = cosmeticManager.cosmetics.AddRangeToArray(customItems.ToArray());
    }
}